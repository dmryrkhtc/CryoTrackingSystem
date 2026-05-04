using FluentValidation;
using CryoTracking.Application.DTOs.Patient;
using CryoTracking.Domain.Enums;
namespace CryoTracking.Application.Validators
{
    public class PatientCreateValidator : AbstractValidator<PatientCreateDto>
    {
        public PatientCreateValidator()
        {
            //  TC No Testi 11 hane ve sadece rakam olmalı
            RuleFor(p => p.TCNo)
                .NotEmpty().WithMessage("TC No boş olamaz.")
                .Length(11).WithMessage("TC Kimlik numarası tam 11 hane olmalıdır.")
                .Matches(@"^[0-9]*$").WithMessage("TC No sadece rakamlardan oluşmalıdır.");

            // İsim Testi En az 3 karakter
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("İsim soyisim alanı zorunludur.")
                .MinimumLength(3).WithMessage("İsim en az 3 karakter olmalıdır.");

            // Doğum Tarihi Testi Gelecek bir tarih olamaz
            RuleFor(p => p.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Doğum tarihi bugünden büyük olamaz.");

            // Medeni Durum Boş geçilmemeli
            RuleFor(p => p.MaritalStatus)
                .NotEmpty().WithMessage("Medeni durum seçilmelidir.");
            //18 yasından kucuk olamaz
            RuleFor(p => p.DateOfBirth)
    .Must(dob => DateTime.Now.Year - dob.Year >= 18)
    .WithMessage("Hasta 18 yaşından küçük olamaz.");
            //evlilik durumu sayı kontrolü
            RuleFor(p => p.MaritalStatus).IsInEnum();
            //es bilgisi evli hastalar için zorunlu
            RuleFor(p => p.PartnerFullName)
    .NotEmpty()
    .When(p => p.MaritalStatus == MaritalStatusType.Married)
    .WithMessage("Evli hastalar için eş bilgisi zorunludur.");
        }
    }
}