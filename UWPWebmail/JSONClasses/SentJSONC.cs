using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UWPWebmail.JSONClasses
{
    class SentJSONC
    {
        public static RootObject_s serialize(string response)
        {
            var serializer = new DataContractJsonSerializer(typeof(RootObject_s));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
            var data = (RootObject_s)serializer.ReadObject(ms);
            return data;
        }
    }

    [DataContract]
    public class E_s
    {
        [DataMember]
        public string a { get; set; }
        [DataMember]
        public string d { get; set; }
        [DataMember]
        public string p { get; set; }
        [DataMember]
        public string t { get; set; }
    }

    [DataContract]
    public class M_s
    {
        [DataMember]
        public int s { get; set; }
        [DataMember]
        public object d { get; set; }
        [DataMember]
        public string l { get; set; }
        [DataMember]
        public string cid { get; set; }
        [DataMember]
        public string f { get; set; }
        [DataMember]
        public int rev { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public List<E_s> e { get; set; }
        [DataMember]
        public string su { get; set; }
        [DataMember]
        public string fr { get; set; }
    }

    [DataContract]
    public class RootObject_s
    {
        [DataMember]
        public List<M_s> m { get; set; }
    }
}
