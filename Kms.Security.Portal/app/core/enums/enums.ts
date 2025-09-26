module nodak.enums {
    export enum TypeOfEntityPage {
        List = 1,//search page
        Edit = 2,
        Insert = 3,
        Display = 3,//datail
        LookUp = 4
    }

    export enum TypeOfController {
        CurdController = 1,
        SearchController = 2
    }
    export enum SubSystems {
        Kms = 1,
        Common = 2,
        Test = 3

    }

    export enum PageType {
        Modal = 1,
        Page = 2
    }

    export enum MessageType {
        Information = 1,
        Error = 2,
        Success = 3,
        Warning = 4
    }

    export enum ServiceTypeEnum {
        Get = 1,
        GetWithId = 2,
        Save = 3,
        GetForGrid = 4,
        Combo = 5,
        Delete = 6,
        Edit = 7,
        GetExelFile = 8,
        SaveOperation = 9,
        GetOperation = 10,
        Dummy = 11
    }

    export enum ShowErrorType {
        UnKnown = 1,
        Model = 2,
        All = 3
    }

    export enum DetailedValidationType {
        Required = 1,
        MaxLength = 2,
        MinLength = 3,
        Email = 4,
        PositiveNum = 5,
        NumberWithDash = 6,
        FloatPositiveNumber = 7,
        FloatNegativeNumber = 8,
        PersianLetters = 9,
        NumberWithEnglishLetter = 10,
        NumberWithPersianLetter = 11,
        CheckNationalityCode = 12,
        LessThanNow = 13,
        NumberWithEnglishLetterWithDashOrUnderScore = 14,
        NotPersianLetter = 15,
        EnglishLetter = 16,
        Other = 17

    }

    export enum MasterValidationType {
        Guard = 1,
        BussinessModelValidation = 2,
        BussinessControllerValidation = 3,
        Specific = 4
    }
    export class NodakCss {
       
        static OKBtn = "kmsOKBtn"
        static CancelBtn = "kmsCancelBtn"
        static Update = "kmsUpdateBtn"
        static Delete = "kmsDeleteBtn"//"btn kmsDeleteBtn";
    }
}