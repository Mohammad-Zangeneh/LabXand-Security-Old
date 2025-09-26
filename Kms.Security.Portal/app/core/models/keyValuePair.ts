module nodak.models {
    export class KeyValuePair<TKey, TValue>{
        Key: TKey;
        Value: TValue;
        constructor(key?: TKey, value?: TValue) {
            if (key != null && value != null) {
                this.Key = key;
                this.Value = value;
            }
        }
    }
}