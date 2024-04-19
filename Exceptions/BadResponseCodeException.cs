namespace HangmanReworked.Exceptions {
    public class BadResponseCodeException : Exception {
        public string URL { get; }
        public BadResponseCodeException() { }

        public BadResponseCodeException(string message) : base(message) { }

        public BadResponseCodeException(string message, Exception innerException) : base(message, innerException) { }

        public BadResponseCodeException(string message, string url) : this(message) { URL = url;}
    }
}