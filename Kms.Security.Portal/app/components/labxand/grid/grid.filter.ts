
interface Function {
    name
}
module labxand.components.core {
    export class FilterBase<T> {
        filterColumns: Array<FilterDefinition>;

        constructor(filters: Array<FilterDefinition>) {
            this.filterColumns = filters;
        }

        CreateFilterFromModel(model: T): Array<FilterSpecification> {
            var filterArray: Array<FilterSpecification> = [];

            for (var i = 0; i < this.filterColumns.length; i++) {
                var filter = new FilterSpecification();
                filter.filterOperation = this.filterColumns[i].FilterOperation;
                filter.propertyName = this.filterColumns[i].PropertyName;
                filter.filterValue = model[this.filterColumns[i].PropertyName];
                filterArray.push(filter);
            }
            return filterArray;
        }
    }

    export class SpecificationOfDataList {
        ascendingSortDirection: boolean;
        filterSpecifications: Array<FilterSpecification>;
        pageIndex: number;
        pageSize: number;
        sortSpecification: string;
    }
    export class SpecificationOfDataListForReport extends SpecificationOfDataList {
        columns: Array<IGridColumns>;
        ExportFormat;
        Url: string;
    }
    export class FilterSpecification {
        filterOperation: any;
        filterValue: any;
        propertyName: string;
    }

    export class FilterDefinition2 {
        filterValue: string;
        FilterOperation: FilterOperations;
    }

    export class FilterSearchModel {
        filters: Array<FilterDefinition2>;
        propertyName: string;

        constructor(propertyName: string) {
            this.propertyName = propertyName;
            this.filters = [];
        }

        IsEqual<TResult>(field: TResult) {
            let filterspec = new FilterDefinition2();
            filterspec.FilterOperation = FilterOperations.Equal;
            //console.log(field)
            //console.log('fielddfsdfsdfsdfsdfsdfsdfsdfds')
            filterspec.filterValue = nodak.PropertyManipulating.GetNameSearchModelGrid(field);
            this.filters.push(filterspec);
            return this;
        }

        IsNotEqual<TResult>(field: TResult) {
            let filterspec = new FilterDefinition2();
            filterspec.FilterOperation = FilterOperations.NotEqual;
            //console.log(field)
            //console.log('fielddfsdfsdfsdfsdfsdfsdfsdfds')
            filterspec.filterValue = nodak.PropertyManipulating.GetNameSearchModelGrid(field);
            this.filters.push(filterspec);
            return this;
        }

        IsLike<TResult>(field: TResult) {
            let filterspec = new FilterDefinition2();
            filterspec.FilterOperation = FilterOperations.Like;
            filterspec.filterValue = nodak.PropertyManipulating.GetNameSearchModelGrid(field);
            this.filters.push(filterspec);
            return this;
        }

        IsLessThan<TResult>(field: TResult) {
            let filterspec = new FilterDefinition2();
            filterspec.FilterOperation = FilterOperations.LessThan;
            filterspec.filterValue = nodak.PropertyManipulating.GetNameSearchModelGrid(field);

            this.filters.push(filterspec);
            return this;
        }

        IsLessThanOrEqual<TResult>(field: TResult) {
            let filterspec = new FilterDefinition2();
            filterspec.FilterOperation = FilterOperations.LessThanOrEqual;
            filterspec.filterValue = nodak.PropertyManipulating.GetNameSearchModelGrid(field);

            this.filters.push(filterspec);
            return this;
        }

        IsGreaterThan<TResult>(field: TResult) {
            let filterspec = new FilterDefinition2();
            filterspec.FilterOperation = FilterOperations.GreaterThan;
            filterspec.filterValue = nodak.PropertyManipulating.GetNameSearchModelGrid(field);
            this.filters.push(filterspec);
            return this;
        }

        IsGreaterThanOrEqual<TResult>(field: TResult) {
            let filterspec = new FilterDefinition2();
            filterspec.FilterOperation = FilterOperations.GreaterThanOrEqual;
            filterspec.filterValue = nodak.PropertyManipulating.GetNameSearchModelGrid(field);
            this.filters.push(filterspec);
            return this;
        }

    }

    export class message {
        Id: string;
        Name: string;
        InsertDate: string;
    }

    export class SearchModelConfiguration<T> {
        protected model;
        protected searchModel;
        filters: Array<FilterSearchModel>;
        constructor(model, searchModel) {
            this.model = model;
            this.searchModel = searchModel;
            this.filters = [];
        }

        Bound<TResult>(Field: TResult) {

            var setF = new FilterSearchModel(nodak.PropertyManipulating.GetNameModelGrid(Field));
            this.filters.push(setF);
            return setF;
        }

        createFilter(model: T): Array<FilterSpecification> {

            var filterArray: Array<FilterSpecification> = [];
            this.filters.forEach((item) => {
                item.filters.forEach((q) => {
                    var filter = new FilterSpecification();

                    filter.propertyName = item.propertyName;

                    let filterValue = model[q.filterValue];

                    let booleanType: boolean = typeof model[q.filterValue] === "boolean";
                    let numberType: boolean = typeof model[q.filterValue] === "number";
                    let checkValue = false;

                    if (booleanType) {
                        checkValue = true;
                    }
                    else if (filterValue != null) {
                        checkValue = true;
                        let str: string = filterValue;
                        if (numberType == false && str.trim() == "")
                            checkValue = false;

                    }

                    if (checkValue) {
                        filter.filterValue = model[q.filterValue];
                        filter.filterOperation = q.FilterOperation;
                        filterArray.push(filter);
                    }

                })

            })

            return filterArray;
        }

    }

    export enum FilterOperations {
        Equal = 1,
        Like = 2,
        NotEqual = 3,
        GreaterThan = 4,
        GreaterThanOrEqual = 5,
        LessThan = 6,
        LessThanOrEqual = 7,
        Between = 8,
    }

    export class FilterDefinition {
        PropertyName: string;
        FilterOperation: FilterOperations;
    }

    export interface IFilterGrid {
        Set(Value: string): string;
    }

    export class GeneralFilter {
        Set(value: string) {
            return value;
        };
    }


    export class BoldItalicFilter implements IFilterGrid {
        Set(value: string) {
            return value;
        };
    }

    export class OnlyTime implements IFilterGrid {
        Set(value: string) {
            return PersianJustTime(value);
        };
    }

    //export class GeogeraphicDMSLat implements IFilterGrid {
    //    Set(value: string) {
    //        let dms = new nodak.directives.ConvertToDMS();
    //        let decimal: number = parseFloat(value);
    //        return dms.ToDMSTextFormat(decimal, "latitude");
    //    }
    //}

    //export class GeogeraphicDMSLong implements IFilterGrid {
    //    Set(value: string) {
    //        let dms = new nodak.directives.ConvertToDMS();
    //        let decimal: number = parseFloat(value);
    //        return dms.ToDMSTextFormat(decimal, "longitude");
    //    }
    //}

    export class ListFilter implements IFilterGrid {
        constructor(private FiledName: string, public count = null) { }
        Set(value: any) {

            //console.log(value);
            if (value) {
                var result = '';
                if (this.count == null) {
                    value.forEach((item) => {

                        result += '"' + item[this.FiledName] + '", ';
                    });
                }
                else {
                    for (var i = 0; i < this.count && i < value.length; i++)
                        result += '"' + value[i][this.FiledName] + '", ';
                    if (this.count < value.length)
                        result += ".....";

                }
                if (result != '')
                    result = result.slice(0, result.length - 2);
                return result;

            }
            else
                return ``
        };
    }
    export class DatePersian implements IFilterGrid {
        Set(value: string) {
            if (!value)
                return;
            return ToPersianDateTimeFromDB(value);
        };

    }
    export class DateWithoutTimePersian implements IFilterGrid {
        Set(value: string) {
            if (!value)
                return;
            return ToPersianDate(value);
        };
    }

    export class DateWithoutTimePersianMonth implements IFilterGrid {
        Set(value: string) {
            if (!value)
                return;
            value = ToPersianDate(value);
            var splits = value.split("/");


            if (splits[2].length == 2) {
                var t = splits[2].split("");
                if (t[0] == "0")
                    t[0] = "";
                splits[2] = t[0] + t[1];
            }

            let Day = splits[2];
            let Month = "";
            switch (splits[1]) {
                case "01":
                    Month = 'فروردین';
                    break;
                case "02":
                    Month = 'اردیبهشت';
                    break;
                case "03":
                    Month = 'خرداد';
                    break;
                case "04":
                    Month = 'تیر';
                    break;
                case "05":
                    Month = 'مرداد';
                    break;
                case "06":
                    Month = 'شهریور';
                    break;
                case "07":
                    Month = 'مهر';
                    break;
                case "08":
                    Month = 'آبان';
                    break;
                case "09":
                    Month = 'آذر';
                    break;
                case "10":
                    Month = 'دی';
                    break;
                case "11":
                    Month = 'بهمن';
                    break;
                case "12":
                    Month = 'اسفند';
                    break;
            }


            return Day + " " + Month;
        };
    }


    export class ThousandSeprate implements IFilterGrid {
        Set(value: string) {
            var x = Number(value);
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }
    }

    export class BooleanField implements IFilterGrid {
        Set(value: string) {
            //console.log(value);
            var booleanValue = Boolean(value);
            if (booleanValue == true)
                return `<span class="glyphicon glyphicon-ok"></span>`

            else if (booleanValue == false)
                return `<span class="glyphicon glyphicon-remove"></span>`
        };

    }

    export class HaveValue implements IFilterGrid {
        Set(value: string) {
            //console.log(value);
            if (value)
                return `<span class="glyphicon glyphicon-ok"></span>`

            else
                return `<span class="glyphicon glyphicon-remove"></span>`
        };

    }


    export class BooleanFieldYesNo implements IFilterGrid {
        Set(value: string) {
            var booleanValue = Boolean(value);
            if (booleanValue == true)
                return `بله`

            else if (booleanValue == false)
                return `خیر`
        };

    }



    export class TopersianDateFilter implements IFilterGrid {
        Set(value: string) {
            return GregorianToPersianDate(value);
        };
    }

    export class TextProperty implements IFilterGrid {
        Set(value: string) {
            return value;
        }
    }


}