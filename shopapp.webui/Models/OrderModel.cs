// Purpose: Contains the OrderModel class which is used to represent the order information in the web application. Also contains the ShipModel, BillModel, and CardModel classes which are used to represent the shipping, billing, and card information in the order.

using System.ComponentModel.DataAnnotations;

namespace shopapp.webui.Models
{
    public class OrderModel
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Phone")]
        [RegularExpression(@"^\+[\d\-\s]+$", ErrorMessage = "Phone number must start with '+' and contain only valid characters.")]
        public string Phone { get; set; }

        [Display(Name = "Order Note")]
        [StringLength(200)]
        public string Note { get; set; }
        public ShipModel ShipModel { get; set; }
        public BillModel BillModel { get; set; }
        public CardModel CardModel { get; set; }
        public CartModel CartModel { get; set; }
    }

    public class ShipModel {

        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your city")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter your country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter your zip code")]
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Please enter a valid zip code.")]
        [StringLength(10)]
        public string ZipCode { get; set; }
    }

    public class BillModel {
        [Required(ErrorMessage = "Please enter address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter city")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter zip code")]
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Please enter a valid zip code.")]
        [StringLength(10)]
        public string ZipCode { get; set; }
    }

    public class CardModel {
        [Required(ErrorMessage = "Please enter name on card")]
        [Display(Name = "Name on Card")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Please enter card number")]
        [Display(Name = "Card Number")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Please enter a valid card number.")]
        [StringLength(16)]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Please enter expiration month")]
        [Display(Name = "Expiration Month")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Please enter a valid expiration month.")]
        [StringLength(2)]
        public string ExpirationMonth { get; set; }

        [Required(ErrorMessage = "Please enter expiration year")]
        [Display(Name = "Expiration Year")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Please enter a valid expiration year.")]
        [StringLength(4)]
        public string ExpirationYear { get; set; }

        [Required(ErrorMessage = "Please enter cvc")]
        [Display(Name = "CVC")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Please enter a valid CVC.")]
        [StringLength(3)]
        public string Cvc { get; set; }
    }
}