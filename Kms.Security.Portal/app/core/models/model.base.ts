module nodak.models {

    export abstract class ModelBase {
        messageHandler: MessageHandler;
        propertyList: Array<nodak.validating.PropertySpecification>;
        propertyName: string;
        elementId: string;
        entityName: string;
        subSystem: nodak.enums.SubSystems;
        validateModelList: Array<string>;
        protected importanatPropertyList: Array<string>;

        constructor(subSystem: nodak.enums.SubSystems, entityName: string) {
            this.propertyList = [];
            this.messageHandler = new MessageHandler();
            this.entityName = entityName;
            this.subSystem = subSystem;
        }

        PropertyForEmptyChecked() {
        }

        ValidateModels() {
            let validateionModel = new nodak.validating.ValidationModel()
            this.validateModelList = validateionModel.modelList;
            return validateionModel;
        }

        ImportantProperties() {
            let validateionModel = new nodak.validating.ValidationModel()
            this.importanatPropertyList = validateionModel.modelList;
            return validateionModel;
        }

        //Set Business Validation
        //Only message with MessageType.Error must be add
        protected abstract AddBussinessModelValidation();

        IsEmpty(): boolean {
            let isEmpty: boolean = false;

            if (this.importanatPropertyList != null) {
                this.importanatPropertyList.forEach((propItem) => {
                    if (this.GetValue(propItem) == null)
                        isEmpty = true;
                });
            }

            return isEmpty;
        }

        Guard<T>(Property: T, Title: string) {
            this.propertyName = nodak.PropertyManipulating.GetName(Property);
            var propertySpecification = new nodak.validating.PropertySpecification(this.propertyName, Title);
            this.propertyList.push(propertySpecification);

            return propertySpecification;
        }

        //validate Guard validation and decide to show realtime error or not 
        IsValidateFor<TResult>(Property: TResult, Value: any, ShowError?: boolean, showInfo?: boolean, showSummary?: boolean) {
            let propName = nodak.PropertyManipulating.GetName(Property);
            var prop = this.propertyList.filter((m) => { return m.ProptertyName == propName });
            if (prop == null) return null;
            var propValidationList = prop[0].ListValidation;
            this.messageHandler.RemoveErrorMessagesByProperty(propName);

            prop[0].ListValidation.forEach((item) => {

                let error: Message = item.CheckValidation(Value, prop[0].Title);
                error.propertyName = propName;
                if (error.valueChanged)
                    Value = error.propertyValue;

                let errorsForThisType = this.messageHandler.GetErrorMessagesByProperty(Property);
                this.messageHandler.RemoveMessages(errorsForThisType);

                if (error && error.text != null) {
                    this.messageHandler.AddMessage(error);
                }

            });

            if (ShowError) {
                var errorForThisProperty: Array<Message> = this.messageHandler.GetErrorMessagesByProperty(Property);

                if (errorForThisProperty.length != 0) {
                    errorForThisProperty.forEach((item) => {
                        nodak.DOMManipulating.RemoveIcon(item.propertyName, item.messageType, this.entityName, this.subSystem);
                        let icon = nodak.DOMManipulating.CreateIcon(item, this.entityName, this.subSystem);
                        nodak.DOMManipulating.Append(icon.Key, icon.Value);
                    })
                }
            }

            if (showInfo) {
                //this.AddMessageForCheckOnSetOfProperties();
                let infoMessages: Array<Message> = this.messageHandler.GetInformationMessagesByProperty(Property);
                infoMessages.forEach((item) => {
                    nodak.DOMManipulating.RemoveIcon(item.propertyName, item.messageType, this.entityName, this.subSystem);
                    let icon = nodak.DOMManipulating.CreateIcon(item, this.entityName, this.subSystem);
                    nodak.DOMManipulating.Append(icon.Key, icon.Value);
                });
            }

            if (showSummary) {
                let infoMessages: Array<Message> = this.messageHandler.GetInformationMessagesByProperty(Property);
                let errorMessages: Array<Message> = this.messageHandler.GetErrorMessagesByProperty(Property);

                if (infoMessages.length != 0) {
                    nodak.DOMManipulating.CreateSummarySection(nodak.enums.MessageType.Information);
                    infoMessages.forEach((item) => {
                        nodak.DOMManipulating.MessageTagBuilder(item, this.entityName, this.subSystem);
                    });
                }

                if (errorMessages.length != 0) {
                    nodak.DOMManipulating.CreateSummarySection(nodak.enums.MessageType.Error);
                    errorMessages.forEach((item) => {
                        nodak.DOMManipulating.MessageTagBuilder(item, this.entityName, this.subSystem);
                    });
                }
            }

            return Value;
        }

        GetValue(propName: string) {
            let value = eval(propName);
            return value;
        }

        IsValid(): boolean {
            this.messageHandler.ClearErrorMessage();
            this.AddBussinessModelValidation();
            if (this.validateModelList != null) {
                this.validateModelList.forEach((modelItem) => {
                    let navigationProperty = modelItem + ".propertyList";
                    modelItem
                    let specifications: Array<nodak.validating.PropertySpecification> = this.GetValue(navigationProperty);
                    specifications.forEach((propItem) => {

                        propItem.ListValidation.forEach((item) => {

                            let field = modelItem + "." + propItem.ProptertyName;
                            let propValue = this.GetValue(field);

                            let errorMessage = item.CheckValidation(propValue, propItem.Title);
                            if (errorMessage.text != null
                                && errorMessage.messageType == nodak.enums.MessageType.Error) {
                                errorMessage.subSystem = this.GetValue(modelItem + ".subSystem");
                                errorMessage.entityName = this.GetValue(modelItem + ".entityName");
                                this.messageHandler.AddMessage(errorMessage);
                            }
                        });

                    });
                })
            }

            //Set GuardValidation
            this.propertyList.forEach((propItem) => {
                propItem.ListValidation.forEach((item) => {
                    debugger;
                    let prop = this[propItem.ProptertyName];

                    let errorMessage = item.CheckValidation(prop, propItem.Title);
                    errorMessage.subSystem = this.subSystem;
                    errorMessage.entityName = this.entityName;
                    if (errorMessage.text != null
                        && errorMessage.messageType == nodak.enums.MessageType.Error) {
                        this.messageHandler.AddMessage(errorMessage);
                        toastr.error(errorMessage.text);
                    }
                });
            });
            //console.log(this.messageHandler.GetErrorMessages());
            return this.messageHandler.GetErrorMessages().length == 0;
        }
    }

    export abstract class ModelBaseSecurityEnabled extends ModelBase {

        constructor(subSystem: nodak.enums.SubSystems, entityName: string) {
            super(subSystem, entityName);
            this.TraceData = new TraceData();
        }

        private traceData: TraceData;
        set TraceData(value) {
            this.traceData = value;
        }
        get TraceData() {
            return this.traceData;
        }
    }

}
