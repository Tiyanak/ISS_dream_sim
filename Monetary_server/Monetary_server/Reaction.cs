using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Monetary_server
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
            this.TaskId = taskId;
            this.MsgType = msgType;
            this.TaskType = taskType;
            this.Incentive = incentive;
            this.ReactionTime = reactionTime;
            this.Threshold = threshold;
        }

        public Reaction(string serializedData)
        {
            Reaction desData = this.Deserialize(serializedData);

            this.TaskId = desData.TaskId;
            this.MsgType = desData.MsgType;
            this.TaskType = desData.TaskType;
            this.Incentive = desData.Incentive;
            this.ReactionTime = desData.ReactionTime;
            this.Threshold = desData.Threshold;

        }

        public string Serialize()
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

        public Reaction Deserialize(string serializedData)
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
            return "Id: " + this.TaskId.ToString() +
                "; MsgType: " + this.MsgType.ToString() + 
                "; Task: " + this.TaskType +
                "; Incentive: " + this.Incentive.ToString() +
                "; Reaction time: " + this.ReactionTime.ToString() +
                "; Threshold: " + this.Threshold.ToString();
        }

        public string getFieldsCSV()
        {
            return "Id,MsgType,Task,Incentive,ReactionTime,Threshold";
        }

        public string getFieldsSemiCSV()
        {
            return "Id;MsgType;Task;Incentive;ReactionTime;Threshold";
        }

        public string toCSV()
        {
            return this.TaskId.ToString() +
                "," + this.MsgType.ToString() +
                "," + this.TaskType +
                "," + this.Incentive.ToString() +
                "," + this.ReactionTime.ToString() +
                "," + this.Threshold.ToString();
        }

        public string toSemiCSV()
        {
            return this.TaskId.ToString() +
                ";" + this.MsgType.ToString() +
                ";" + this.TaskType +
                ";" + this.Incentive.ToString() +
                ";" + this.ReactionTime.ToString() +
                ";" + this.Threshold.ToString();
        }
    }
}
