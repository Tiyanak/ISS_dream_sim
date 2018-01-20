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
    public class Parameters
    {
        public long taskId;
        public int msgType;
        public double targetDisplayTime;
        public double cueToTargetTime;
        public double threshold;

        public Parameters() { }

        public Parameters(long taskId, int msgType, double targetDisplayTime, double cueToTargetTime, double threshold)
        {
            this.taskId = taskId;
            this.msgType = msgType;
            this.targetDisplayTime = targetDisplayTime;
            this.cueToTargetTime = cueToTargetTime;
            this.threshold = threshold;
        }

        public Parameters(string serializedData)
        {
            Parameters des = (Parameters)this.Deserialize(serializedData);

            this.taskId = des.taskId;
            this.msgType = des.msgType;
            this.targetDisplayTime = des.targetDisplayTime;
            this.cueToTargetTime = des.cueToTargetTime;
            this.threshold = des.threshold;
        }

        public string Serialize()
        {
            string serializedData = string.Empty;

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
            Parameters deserializedparameters = new Parameters();

            XmlSerializer deserializer = new XmlSerializer(typeof(Parameters));
            using (TextReader tr = new StringReader(serializedData))
            {
                deserializedparameters = (Parameters)deserializer.Deserialize(tr);
            }

            return deserializedparameters;
        }

        public override string ToString()
        {
            return "TaskId: " + this.taskId.ToString() +
                "; MsgType: " + this.msgType.ToString() + 
                "; TargetDisplayTime: " + this.targetDisplayTime.ToString() +
                "; CueToTargetTime: " + this.cueToTargetTime.ToString() +
                "; Threshold: " + this.threshold.ToString();
        }
        
    }
}
