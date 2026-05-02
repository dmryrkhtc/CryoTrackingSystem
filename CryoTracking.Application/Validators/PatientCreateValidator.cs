using FluentValidation;
using CryoTracking.Application.DTOs.Patient;

namespace CryoTracking.Application.Validators
{
    public class PatientCreateValidator : AbstractValidator<PatientCreateDto>
    {
        public PatientCreateValidator()
        {
            // 1. TC No Testi: 11 hane ve sadece rakam olmalı
            RuleFor(p => p.TCNo)
                .NotEmpty().WithMessage("TC No boş olamaz.")
                .Length(11).WithMessage("TC Kimlik numarası tam 11 hane olmalıdır.")
                .Matches(@"^[0-9]*$").WithMessage("TC No sadece rakamlardan oluşmalıdır.");

            // 2. İsim Testi: En az 3 karakter
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("İsim soyisim alanı zorunludur.")
                .MinimumLength(3).WithMessage("İsim en az 3 karakter olmalıdır.");

            // 3. Doğum Tarihi Testi: Gelecek bir tarih olamaz
            RuleFor(p => p.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Doğum tarihi bugünden büyük olamaz.");

            // 4. Medeni Durum: Boş geçilmemeli
            RuleFor(p => p.MaritalStatus)
                .NotEmpty().WithMessage("Medeni durum seçilmelidir.");
        }
    }
}