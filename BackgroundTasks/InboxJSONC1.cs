using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks
{
    sealed class InboxJSONC1
    {

        public static RootObject1 serialize(string response)
        {
            var serializer = new DataContractJsonSerializer(typeof(RootObject1));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
            var data = (RootObject1)serializer.ReadObject(ms);
            return data;
        }
    }

    [DataContract]
    public sealed class E1
    {
        [DataMember]
        public string a { get; set; }
        [DataMember]
        public string d { get; set; }
        [DataMember]
        public string t { get; set; }
        [DataMember]
        public string p { get; set; }
    }

    [DataContract]
    public sealed class M1
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
        public int rev { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public IList<E1> e { get; set; }
        [DataMember]
        public string su { get; set; }
        [DataMember]
        public string fr { get; set; }
        [DataMember]
        public string f { get; set; }
    }

    [DataContract]
    public sealed class RootObject1
    {
        [DataMember]
        public IList<M1> m { get; set; }
    }
}
