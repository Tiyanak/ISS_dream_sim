using System;
using System.IO;
using System.Xml.Serialization;

namespace Assets.Scripts.Classes.Msgs
{
    [Serializable]
    public class Parameters
    {
        public readonly long TaskId;
        public readonly int MsgType;
        public readonly double TargetDisplayTime;
        public readonly double CueToTargetTime;
        public readonly double Threshold;

        public Parameters() { }

        public Parameters(long taskId, int msgType, double targetDisplayTime, double cueToTargetTime, double threshold)
        {
            TaskId = taskId;
            MsgType = msgType;
            TargetDisplayTime = targetDisplayTime;
            CueToTargetTime = cueToTargetTime;
            Threshold = threshold;
        }

        public Parameters(string serializedData)
        {
            Parameters des = Deserialize(serializedData);

            TaskId = des.TaskId;
            MsgType = des.MsgType;
            TargetDisplayTime = des.TargetDisplayTime;
            CueToTargetTime = des.CueToTargetTime;
            Threshold = des.Threshold;
        }

        public string Serialize()
        {
            string serializedData;

            XmlSerializer serializer = new XmlSerializer(typeof(Parameters));
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, this);
                serializedData = sw.ToString();
            }

            return serializedData;
        }

        public Parameters Deserialize(string serializedData)
        {
            Parameters deserializedparameters;

            XmlSerializer deserializer = new XmlSerializer(typeof(Parameters));
            using (TextReader tr = new StringReader(serializedData))
            {
                deserializedparameters = (Parameters)deserializer.Deserialize(tr);
            }

            return deserializedparameters;
        }

        public override string ToString()
        {
            return "TaskId: " + TaskId +
                "; MsgType: " + MsgType +
                "; TargetDisplayTime: " + TargetDisplayTime +
                "; CueToTargetTime: " + CueToTargetTime +
                "; Threshold: " + Threshold;
        }

    }
}
