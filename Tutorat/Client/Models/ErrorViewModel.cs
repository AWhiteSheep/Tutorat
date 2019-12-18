using System;

namespace Client.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Url { get; set; }
        public string Message { get; set; }
        public string Titre { get; set; }

    }
}
