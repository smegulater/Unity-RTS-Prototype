
public class Metric 
{

        public string Name { get { return name; } }
        public object Data { get { return data; } set { data = value; } }

        private string name;
        private object data;

        public Metric(string Name, object Data)
        {
            name = Name;
            data = Data;
        }

}
