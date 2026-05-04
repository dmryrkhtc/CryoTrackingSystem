namespace CryoTracking.Domain.Enums;

public enum StatusType
{
    Frozen = 1,
    Thawed = 2,
    Discarded = 3,
    LegalHold = 4 
}

public enum SampleType
{
    Embryo = 1,
    Sperm = 2,
    Egg = 3
}

public enum Gender
{
    Female = 1,
    Male = 2
  
}
//avukatı ilgilendiriyor
public enum MaritalStatusType { Single = 1, Married = 2, Divorced = 3, Widowed = 4 } //dul vb
//doktoru ilgilendiriyor
public enum CoupleType { Single = 1, Married = 2, Consanguineous = 3 } // Akraba evliliği vb.