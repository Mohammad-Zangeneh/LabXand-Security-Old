module nodak.directives {
    export interface IFileUpload extends ng.IScope {
        Id: string;
        buttonId: string;
        filesSection: string;
        clearId: string;
        files: any;
        //  selected: imas.common.models.BlobDescription;
        download(): any;
        fileBrowse: any;
        removeItem(e): any;
        http: ng.IHttpService;
        downloadFile(e): any;
        checking: boolean;

        determineContentType: any;

        fileUpload: nodak.common.models.AttachmentDescription;

        justOne: boolean;
        justPicture: boolean;
    }
    export function FileUpload($compile, $timeout, $http): ng.IDirective {
        return {
            template: `
                        <input type="file" ng-if="justPicture"  accept="image/png, image/jpeg, image/gif" id="{{buttonId}}" style="display:none" />
                        <input type="file" ng-if="!justPicture"  id="{{buttonId}}" style="display:none" />
                      
                          <div class="file-preview">
                            <div id="{{clearId}}">
                                <div id="{{filesSection}}"></div>
                            </div>
                        </div>
                        <a class="btn btn-sm btn-primary" ng-click="fileBrowse()">انتخاب فایل</a>`,
            restrict: 'E',
            scope: {
                fileUpload: '=fileUpload',
                justOne: "=",
                justPicture:"="
            },
            link: function (scope: IFileUpload, element, attrs) {
                
              

                scope.checking = false;
                scope.buttonId = 'fu' + Math.random();
                scope.filesSection = "files" + Math.random();
                scope.clearId = "clear" + Math.random();

                ///for update
                scope.$watchCollection(() => { return scope.fileUpload.Attachments }, (newValue, oldValue) => {
                    //for add after new blobDescriptions
                    if (newValue == null || newValue.length == 0) {

                        document.getElementById(scope.clearId).innerHTML = "";

                        var div = document.createElement('div');
                        div.setAttribute('id', scope.filesSection);
                        document.getElementById(scope.clearId).appendChild(div);

                    }

                    if (newValue != null /*&& scope.checking == false*/) {
                        

                        if (scope.fileUpload.Attachments) {
                            angular.element(document.getElementById(scope.filesSection)).empty();
                            for (var i = 0; i < scope.fileUpload.Attachments.length; i++) {

                                var namev = scope.fileUpload.Attachments[i].FilePath;
                                var idBlob = (scope.fileUpload.Attachments[i].Id);

                                scope.fileUpload.Attachments[i].ItemId = i;

                                var Src = scope.determineContentType(scope.fileUpload.Attachments[i].MIME);

                                let ele = document.getElementById(scope.Id + 'blob' + + i);
                                if (!ele) {

                                    angular.element(document.getElementById(scope.filesSection)).append($compile(

                                        '<div id="blob' + i + '"  >' +

                                        '<div class="file-preview-frame" >' +
                                        '<div class="img">' +
                                        '<img src="' + Src + '" title="' + namev + '"/>' +

                                        '<div class="desc row">' + namev + '</div>' +
                                        '<div id="glyphicon" >' +
                                        '<a class="glyphicon glyphicon-trash text-danger col-md-2 " ng-click="removeItem(' + i + ')" title="حذف"></a>' +
                                        ' <a class="glyphicon glyphicon-download-alt col-md-2 " id="download' + i + '" ng-click="downloadFile( ' + i + ' )"  title="دانلود" > </a >' +
                                        '</div>' +

                                        '</div></div></div>')(scope));
                                }
                            }
                        }
                    }
                })

                ////////end or update
                scope.downloadFile = (e) => {
                    let href1;
                    let blobSelected = scope.fileUpload.Attachments.filter((d) => { return d.ItemId == e });
                    //console.log('blobSelected[0]', blobSelected[0])
                    let idBlob = blobSelected[0].Id;
                    let update = document.getElementById("download" + e);
                    if (!update.hasAttribute("href")) {
                        
                        new nodak.common.services.AttachmentService($http).Post(idBlob, nodak.enums.ServiceTypeEnum.GetWithId).then((model) => {

                            $timeout(function () {
                                let contentType = "file" + e;
                                var blobURL;
                                if (model != null && model != undefined && model != "") {
                                    let blobFromBlobFile = b64toBlob(model, contentType, 512);
                                    blobURL = URL.createObjectURL(blobFromBlobFile);

                                }
                                else if (blobSelected[0].AttachmentURL != "")
                                    blobURL = blobSelected[0].AttachmentURL;

                                function b64toBlob(b64Data, contentType, sliceSize) {
                                    contentType = contentType || '';
                                    sliceSize = sliceSize || 512;

                                    var byteCharacters = atob(b64Data);
                                    var byteArrays = [];

                                    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                                        var slice = byteCharacters.slice(offset, offset + sliceSize);

                                        var byteNumbers = new Array(slice.length);
                                        for (var i = 0; i < slice.length; i++) {
                                            byteNumbers[i] = slice.charCodeAt(i);
                                        }

                                        var byteArray = new Uint8Array(byteNumbers);

                                        byteArrays.push(byteArray);
                                    }

                                    var blob = new Blob(byteArrays, { type: contentType });
                                    //console.log(blob);
                                    return blob;
                                }

                                update.setAttribute("href", blobURL);
                                update.setAttribute("download", blobSelected[0].FilePath);

                                update.click();
                            });

                        });

                    }
                }

                scope.removeItem = (e) => {
                    let index = scope.fileUpload.Attachments.indexOf(
                        scope.fileUpload.Attachments.filter((filterModel) => {
                            return filterModel.ItemId == e;
                        })[0])
                    scope.fileUpload.Attachments.splice(index, 1);
                };

                var files;
                scope.fileBrowse = () => {
                    document.getElementById(scope.buttonId).click();
                };

                scope.determineContentType = function (contentType) {
                    let src;
                    switch (contentType) {
                        case "image/jpeg":
                            src = "/Content/components/fileUpload/images.jpg";
                            break;
                        case "image/png":

                            src = "/Content/components/fileUpload/images.jpg";

                            break;
                        case "application/pdf":

                            src = "/Content/components/fileUpload/pdf.png";

                            break;
                        case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":

                            src = "/Content/components/fileUpload/Excel.png";
                            break;

                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                            src = "/Content/components/fileUpload/text.jpg";
                            break;

                        case "text/plain":
                            src = "/Content/components/fileUpload/text.jpg";
                            break;

                        default:
                            src = "/Content/components/fileUpload/General.png";
                    }

                    return src;
                }

                element.bind('change', function (e) {
                    
                    scope.checking = true;
                    scope.Id = "fileInput" + Math.random();
                    files = e.target.files;
                    var name = e.target.files[0].name;
                    var size = e.target.files[0].size;
                    var contentType = e.target.files[0].type;
                  
                    if (scope.justOne && scope.fileUpload.Attachments != null && scope.fileUpload.Attachments.length == 1) {
                        alert("فقط یک عکس می توانید پیوست کنید.");
                        return;
                    }
                    for (var i = 0; i < e.target.files.length; i++) {
                        var reader = new FileReader();
                        reader.onload = function () {
                            var blob = new nodak.common.models.Attachment();
                            var id;
                            //blob.Id = id;
                            if (scope.fileUpload.Attachments)
                                blob.ItemId = scope.fileUpload.Attachments.length;
                            else {
                                blob.ItemId = 0;
                                scope.fileUpload.Attachments = [];
                            }

                            blob.ItemId = scope.fileUpload.Attachments.length;

                            blob.FilePath = name;
                            blob.FileData = btoa(reader.result);
                            blob.MIME = contentType;
                            scope.fileUpload.Attachments.push(blob);

                            let href, src;

                            src = scope.determineContentType(contentType);

                            var byteCharacters = btoa(reader.result);
                            var blobFromBlobFile = b64toBlob(byteCharacters, contentType, 512);
                            var blobURL = URL.createObjectURL(blobFromBlobFile);


                            blob.AttachmentURL = blobURL;

                            function b64toBlob(b64Data, contentType, sliceSize) {
                                contentType = contentType || '';
                                sliceSize = sliceSize || 512;

                                var byteCharacters = atob(b64Data);
                                var byteArrays = [];

                                for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                                    var slice = byteCharacters.slice(offset, offset + sliceSize);

                                    var byteNumbers = new Array(slice.length);
                                    for (var i = 0; i < slice.length; i++) {
                                        byteNumbers[i] = slice.charCodeAt(i);
                                    }

                                    var byteArray = new Uint8Array(byteNumbers);

                                    byteArrays.push(byteArray);
                                }

                                var blob = new Blob(byteArrays, { type: contentType });
                                return blob;

                            }

                            angular.element(document.getElementById(scope.filesSection)).append($compile(

                                '<div id="blob' + +blob.ItemId + '"  >' +

                                '<div class="file-preview-frame" >' +

                                '<div class="img">' +
                                '<div class="row"><img src="' + src + '" title="' + name + '"/></div>' +

                                '<div class="desc row">' + name + '</div>' +

                                '<a class="glyphicon glyphicon-trash text-danger col-md-2  " ng-click="removeItem(' + blob.ItemId + ')" title="حذف"></a>' +
                                ' <a class="glyphicon glyphicon-download-alt col-md-2 "  id="download12" href=' + blobURL + ' download="' + name + '"  title="دانلود"> </a >' +


                                '</div></div></div></div>')(scope));
                        }

                        reader.readAsBinaryString(files[i]);
                    }

                });
            }


        }
    }

}

angular.module('nodak.directives').directive('fileUpload', nodak.directives.FileUpload);
