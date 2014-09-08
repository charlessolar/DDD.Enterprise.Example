// https://github.com/elijahmanor/amplify-request-deferred/blob/master/src/amplify.request.deferred.js

(function (amplify, $, undefined) {
	var properties = ["types", "resources", "define", "decoders"];

	amplify.request_original = amplify.request;
	amplify.request = function (resourceId, data) {
		var dfd = $.Deferred();

		amplify.request_original({
			resourceId: resourceId,
			data: data,
			success: dfd.resolve,
			error: dfd.reject
		});

		return dfd.promise();
	};

	$.each(properties, function (index, key) {
		amplify.request[key] = amplify.request_original[key];
	});
})(amplify, jQuery);