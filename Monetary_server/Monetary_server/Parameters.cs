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
        public long TaskId;
        public int MsgType;
        public double TargetDisplayTime;
        public double CueToTargetTime;
        public double Threshold;

        public Parameters() { }

        public Parameters(long taskId, int msgType, double targetDisplayTime, double cueToTargetTime, double threshold)
        {
            this.TaskId = taskId;
            this.MsgType = msgType;
            this.TargetDisplayTime = targetDisplayTime;
            this.CueToTargetTime = cueToTargetTime;
            this.Threshold = threshold;
        }

        public Parameters(string serializedData)
        {
            Parameters des = (Parameters)this.Deserialize(serializedData);

            this.TaskId = des.TaskId;
            this.MsgType = des.MsgType;
            this.TargetDisplayTime = des.TargetDisplayTime;
            this.CueToTargetTime = des.CueToTargetTime;
            this.Threshold = des.Threshold;
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
            return "TaskId: " + this.TaskId.ToString() +
                "; MsgType: " + this.MsgType.ToString() + 
                "; TargetDisplayTime: " + this.TargetDisplayTime.ToString() +
                "; CueToTargetTime: " + this.CueToTargetTime.ToString() +
                "; Threshold: " + this.Threshold.ToString();
        }
        
    }
}
