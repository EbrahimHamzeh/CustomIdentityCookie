using System;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace IdentityCookie.Common
{
    public static class ValidationExtensions
    {
        public static string GetValidationErrors(this DbContext conext)
        {
            var errors = new StringBuilder();
            var entities = conext.ChangeTracker.Entries()
                            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                            .Select(e => e.Entity);
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(entity, validationContext, validationResults, 
                validateAllProperties: true))
                {
                    foreach (var validationResult in validationResults)
                    {
                        var names = validationResult.MemberNames.Aggregate((s1, s2) => $"{s1}, {s2}");
                        errors.AppendFormat("{0}: {1}", names, validationResult.ErrorMessage);
                    }
                }
            }
            
            return errors.ToString();
        }
    }
}
