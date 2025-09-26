interface Toast {
    success(message: string, title?: string);
    error(message: string, title?: string);
    info(message: string, title?: string);
    warning(message: string, title?: string);
    options: any;
}


var toastr: Toast;