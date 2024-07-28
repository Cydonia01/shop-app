/*
* Card.cs is a class that contains the properties of a card.
* It is used to define the Card class.
*/
namespace shopapp.entity
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvc { get; set; }

    }
}