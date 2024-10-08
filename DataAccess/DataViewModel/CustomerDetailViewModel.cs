﻿using System.ComponentModel.DataAnnotations;

public class CustomerDetailViewModel
{
    [Required]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters ")]

    public string Name { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?=.*\S)[a-zA-Z0-9\s]*$", ErrorMessage = "Postcode can only contain alphanumeric characters ")]
    public string Postcode { get; set; } = string.Empty;

    [Required][RegularExpression(@"^(?=.*\S)[a-zA-Z\s]*$", ErrorMessage = "Country can only contain letters ")] public string Country { get; set; } = string.Empty;

    [Required][RegularExpression(@"^[+]{1}\d{10}$", ErrorMessage = "Must be in (+911234567890) format")] public string Telephone { get; set; } = string.Empty;

    [Required][RegularExpression(@"^(?=.*\S)[a-zA-Z\s]*$", ErrorMessage = "Relation can only contain letters ")] public string Relation { get; set; } = string.Empty;

    [Required][RegularExpression(@"^(?=.*\S)[a-zA-Z\s]*$", ErrorMessage = "Currency can only contain letters ")] public string Currency { get; set; } = string.Empty;

    [Required]
    public string Address1 { get; set; } = string.Empty;

    [Required]
    public string Address2 { get; set; } = string.Empty;

    [Required][RegularExpression(@"^(?=.*\S)[a-zA-Z\s]*$", ErrorMessage = "Town can only contain letters ")] public string Town { get; set; } = string.Empty;

    [Required][RegularExpression(@"^(?=.*\S)[a-zA-Z\s]*$", ErrorMessage = "County can only contain letters ")] public string County { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;

    public bool? Issubscribe { get; set; }

    /*[Required][RegularExpression(@"^[A-Z]{2}\d{5}$", ErrorMessage = "Ac must start with two uppercase letters followed by five digits.")]*/ public string Ac { get; set; } = string.Empty;

    public int Id { get; set; }
}
