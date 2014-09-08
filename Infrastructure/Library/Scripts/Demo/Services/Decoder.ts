module Demo.Library.Services {
    import Responses = Demo.Library.Responses;

    amplify.request.decoders['full'] =  (data?: any, status?: string, xhr?: JQueryXHR, success?: (...args: any[]) => void, error?: (...args: any[]) => void) => {
        if (data.Status === "success") {
            success(data.Payload);
        } else if (data.Status === "fail" || data.Status === "error") {
            error(data.Message, data.Status);
        } else {
            error(data.Message, "fatal");
        }
    }
} 