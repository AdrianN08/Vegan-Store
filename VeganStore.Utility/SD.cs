namespace VeganStore.Utility
{
    public static class SD
    {
        public const string localHost = "https://localhost:44316/api/";
        public const string ApiKey = "key=d78769f3af0f4546b3d6a9ead9ce5eed";

        public const string StatusPending = "Väntar";
        public const string StatusApproved = "Godkänd";
        public const string StatusInProcess = "Pågående";
        public const string StatusShipped = "Skickad";
        public const string StatusCancelled = "Avbruten";
        public const string StatusRefunded = "Återkallad";

        public const string PaymentStatusPending = "Väntar";
        public const string PaymentStatusApproved = "Betald";
        public const string PaymentStatusDelayedPayment = "Godkänd Sen Betalning";
        public const string PaymentStatusRejected = "Nekad";

    }
}