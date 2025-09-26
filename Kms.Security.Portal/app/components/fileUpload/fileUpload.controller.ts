//appModule.controller('FileUploadController', Models.FileUpload);
module nodak.components.controllers {
    export class fileUploadController {
        currentModel: models.FileUpload;
       
        constructor() {
            this.currentModel = new models.FileUpload();
     
        }
    }
}

componentsControllersModule.controller('components.fileUploadController', nodak.components.controllers.fileUploadController);