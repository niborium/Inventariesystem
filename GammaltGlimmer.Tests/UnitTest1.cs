using Xunit;
using GammaltGlimmer.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GammaltGlimmer.Tests;

public class GammaltGlimmerTests
{
    [Fact]
    public void RequiredFieldsValidationTest()
    {
        /*Comment out Name that is required field (see below) and Validating my model.
        should be Count 1 as we have one error.*/
        Item add = new()
        {
            ItemId = "TTT123456",
            //Name = "xUnitTest",
            Description = "xUnitTest",
            ReleaseYear = 2021,
            StartPrice = 20,
            PurchaseCost = 10,
            CategoryId = 1
        };
        var lstErrors = ValidateModel(add);
        Assert.True(lstErrors.Count == 1);
    }
    [Fact]
    public void ReqularExpressionValidationTest()
    {
        /* Puts in incorrect ItemId (see below) should get error message, should be Count 1 as we have one error.*/
        Item add = new()
        {
            ItemId = "TTT123", //Not correct - should be 3 letters and 6 numbers.
            Name = "xUnitTest",
            Description = "xUnitTest",
            ReleaseYear = 2021,
            StartPrice = 20,
            PurchaseCost = 10,
            CategoryId = 1
        };
        var lstErrors = ValidateModel(add);
        Assert.True(lstErrors.Count(x => x.ErrorMessage.Contains("ID måste bestå av tre bokstäver och sex siffror (Exempelvis: TTT111111)")) == 1);
    }
    [Fact]
    public void GreaterThanValidationTest()
    {
        /* Puts in lower StartPrice (see below) then PurchaseCost
        should get error message, should be Count 1 as we have one error.*/
        Item add = new()
        {
            ItemId = "TTT123456",
            Name = "xUnitTest",
            Description = "xUnitTest",
            ReleaseYear = 2021,
            StartPrice = 5, //Lower than PurchaseCost, will trigger error message.
            PurchaseCost = 10,
            CategoryId = 1,
            CreatedBy = "System"
        };
        var lstErrors = ValidateModel(add);
        Assert.True(lstErrors.Count(x => x.ErrorMessage.Contains("Måste vara högre än Inköpskostnaden")) == 1);
    }
    private static List<ValidationResult> ValidateModel<T>(T model)
    {
        var context = new ValidationContext(model, null, null);
        var result = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(model, context, result, true);
        return result;
    }
}