using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.Scripts.Classes.Msgs
{
    [Serializable]
    public class Reaction
    {

        public long taskId;
        public int msgType;
        public string taskType;
        public bool incentive;
        public double reactionTime;
        public double threshold;

        public Reaction() { }

        public Reaction(long taskId, int msgType, string taskType, bool incentive, double reactionTime, double threshold)
        {
            this.taskId = taskId;
            this.msgType = msgType;
            this.taskType = taskType;
            this.incentive = incentive;
            this.reactionTime = reactionTime;
            this.threshold = threshold;
        }

        public Reaction(string serializedData)
        {
            Reaction desData = this.deserialize(serializedData);

            this.taskId = desData.taskId;
            this.msgType = desData.msgType;
            this.taskType = desData.taskType;
            this.incentive = desData.incentive;
            this.reactionTime = desData.reactionTime;
            this.threshold = desData.threshold;

        }

        public string serialize()
        {

            string serializedData = string.Empty;

            XmlSerializer serializer = new XmlSerializer(typeof(Reaction));
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, this);
                serializedData = sw.ToString();
            }

            return serializedData;
        }

        public Reaction deserialize(string serializedData)
        {

            Reaction deserializedReaction = new Reaction();

            XmlSerializer deserializer = new XmlSerializer(typeof(Reaction));
            using (TextReader tr = new StringReader(serializedData))
            {
                deserializedReaction = (Reaction)deserializer.Deserialize(tr);
            }

            return deserializedReaction;
        }

        public override string ToString()
        {
            return "Id: " + this.taskId.ToString() +
                "; MsgType: " + this.msgType.ToString() +
                "; Task: " + this.taskType +
                "; Incentive: " + this.incentive.ToString() +
                "; Reaction time: " + this.reactionTime.ToString() +
                "; Threshold: " + this.threshold.ToString();
        }

        public string getFieldsCSV()
        {
            return "Id,MsgType,Task,Incentive,ReactionTime,Threshold";
        }

        public string toCSV()
        {
            return this.taskId.ToString() +
                "," + this.msgType.ToString() +
                "," + this.taskType +
                "," + this.incentive.ToString() +
                "," + this.reactionTime.ToString() +
                "," + this.threshold.ToString();
        }
    }
}
