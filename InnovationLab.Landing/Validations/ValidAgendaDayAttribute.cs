using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.DbContexts;
using InnovationLab.Landing.Models;

namespace InnovationLab.Landing.Validations;

public class ValidAgendaDayAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not int day || day < 1)
            return new ValidationResult("Day must be at least 1.");

        if (validationContext.ObjectInstance is not EventAgenda agenda)
            return ValidationResult.Success;

        if (validationContext.GetService(typeof(LandingDbContext)) is not LandingDbContext dbContext)
            return ValidationResult.Success;

        var ev = dbContext.Events.Find(agenda.EventId);
        if (ev == null)
            return new ValidationResult("Event not found.");

        var eventDays = (ev.EndTime.Date - ev.StartTime.Date).Days + 1;
        if (day > eventDays)
            return new ValidationResult($"Day must be between 1 and {eventDays} for this event.");

        // Uniqueness check
        var exists = dbContext.EventAgendas.Any(a => a.EventId == agenda.EventId && a.Day == day && a.Id != agenda.Id);
        if (exists)
            return new ValidationResult($"Agenda for day {day} already exists for this event.");

        return ValidationResult.Success;
    }
}