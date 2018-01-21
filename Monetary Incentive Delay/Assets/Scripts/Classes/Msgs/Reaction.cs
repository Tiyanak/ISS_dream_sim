using System;
using System.IO;
using System.Xml.Serialization;

namespace Assets.Scripts.Classes.Msgs
{
    [Serializable]
    public class Reaction
    {

        public long TaskId;
        public int MsgType;
        public string TaskType;
        public bool Incentive;
        public double ReactionTime;
        public double Threshold;

        public Reaction() { }

        public Reaction(long taskId, int msgType, string taskType, bool incentive, double reactionTime, double threshold)
        {
            TaskId = taskId;
            MsgType = msgType;
            TaskType = taskType;
            Incentive = incentive;
            ReactionTime = reactionTime;
            Threshold = threshold;
        }

        public Reaction(string serializedData)
        {
            Reaction desData = Deserialize(serializedData);

            TaskId = desData.TaskId;
            MsgType = desData.MsgType;
            TaskType = desData.TaskType;
            Incentive = desData.Incentive;
            ReactionTime = desData.ReactionTime;
            Threshold = desData.Threshold;

        }

        public string Serialize()
        {

            string serializedData;

            XmlSerializer serializer = new XmlSerializer(typeof(Reaction));
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, this);
                serializedData = sw.ToString();
            }

            return serializedData;
        }

        public Reaction Deserialize(string serializedData)
        {

            Reaction deserializedReaction;

            XmlSerializer deserializer = new XmlSerializer(typeof(Reaction));
            using (TextReader tr = new StringReader(serializedData))
            {
                deserializedReaction = (Reaction)deserializer.Deserialize(tr);
            }

            return deserializedReaction;
        }

        public override string ToString()
        {
            return "Id: " + TaskId +
                "; MsgType: " + MsgType +
                "; Task: " + TaskType +
                "; Incentive: " + Incentive +
                "; Reaction time: " + ReactionTime +
                "; Threshold: " + Threshold;
        }

        public string GetFieldsCsv()
        {
            return "Id,MsgType,Task,Incentive,ReactionTime,Threshold";
        }

        public string GetFieldsSemiCsv()
        {
            return "Id;MsgType;Task;Incentive;ReactionTime;Threshold";
        }

        public string ToCsv()
        {
	        return TaskId +
	               "," + MsgType +
	               "," + TaskType +
	               "," + Incentive +
	               "," + ReactionTime +
	               "," + Threshold;
        }

        public string ToSemiCsv()
        {
            return TaskId +
                ";" + MsgType +
                ";" + TaskType +
                ";" + Incentive +
                ";" + ReactionTime +
                ";" + Threshold;
        }
    }
}
