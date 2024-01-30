namespace API
{
    public class Sender { 
            public string email { get; set;}
            public string code { get; set;}
            public DateTime date { get; set;}
       public Sender() {
            email = "";
            code = "";
            date = default;
        }
        public Sender(string email, string code, DateTime date)
        {
            this.email = email;
            this.code = code;
            this.date = date;
        }
    }
}
