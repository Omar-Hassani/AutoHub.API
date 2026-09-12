using System;
using FluentValidation;
using AutoHub.API.DTOs;

namespace AutoHub.API.Validators
{
    public class CarCreateUpdateDtoValidator : AbstractValidator<CarCreateUpdateDto>
    {
        public CarCreateUpdateDtoValidator()
        {
            RuleFor(x => x.Make)
                .NotEmpty().WithMessage("ماركة السيارة (Make) مطلوبة.")
                .MaximumLength(50).WithMessage("اسم الماركة يجب ألا يتجاوز 50 حرفاً.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("موديل السيارة (Model) مطلوب.")
                .MaximumLength(50).WithMessage("اسم الموديل يجب ألا يتجاوز 50 حرفاً.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.Now.Year + 1)
                .WithMessage($"سنة الصنع يجب أن تكون بين 1900 و {DateTime.Now.Year + 1}.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("سعر السيارة يجب أن يكون أكبر من 0.");

            RuleFor(x => x.Condition)
                .IsInEnum()
                .WithMessage("حالة السيارة غير صالحة (يجب أن تكون قيمة مقبولة من CarCondition).");

            RuleFor(x => x.Mileage)
                .GreaterThanOrEqualTo(0)
                .WithMessage("المسافة مقطوعة (Mileage) لا يمكن أن تكون بالسالب.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("الوصف يجب ألا يتجاوز 1000 حرف.");
        }
    }
}