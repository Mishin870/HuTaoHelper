using HuTaoHelper.Notifications.Target;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuTaoHelper.Notifications.Registry;

public class NotificationJsonConverter : JsonConverter {
	public override bool CanConvert(Type objectType) {
		return typeof(INotificationTarget).IsAssignableFrom(objectType);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
		JsonSerializer serializer) {
		var source = JObject.Load(reader);
		var type = (string?)source["#type#"];

		if (type == null) {
			throw new ArgumentException("Notification target type is null");
		} else {
			var target = NotificationsRegistry.Build(type);
			serializer.Populate(source.CreateReader(), target);
			return target;
		}
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
		var output = new JObject();

		if (value is INotificationTarget target) {
			output.AddFirst(new JProperty("#type#", target.NotificationType()));
		}
		
		foreach (var prop in value.GetType().GetProperties()) {
			if (!prop.CanRead) continue;
			
			var propValue = prop.GetValue(value);
			if (propValue != null) {
				output.Add(prop.Name, JToken.FromObject(propValue, serializer));
			}
		}

		output.WriteTo(writer);
	}
}