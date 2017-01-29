var URL = function(url) {
	this.url = url;

	this.params = (function(URL) {
		
		var p = { };
		
		if (this.indexOf('?') < 0 && this.indexOf('#') < 0) return p;
		
		var query;
		
		if (this.indexOf('?') < 0)
		{
			query = this.split('#')[1];
		}
		else
		{
			query = this.split('?')[1];
		}
		
		query = query.replace(/\+/g, ' ');

		var pairs = query.split('&');
		
		if (pairs === undefined || pairs.length == 0) return p;
		
		for(var i = 0; i < pairs.length; i++) {
			var pair = pairs[i].trim();
			
			if (pair.length == 0) continue;
			
			if (pair.indexOf('=') < 0) {
				p[pair[0]] = '';
			}
			else {
				
				if (pair.indexOf('#') === 0)
					pair = pair.substr(1);
				
				var prop = pair.split('=');
				
				p[prop[0]] = prop[1].replace(/\_/g, ' ');
			}
		}
		
		return p;
		
	}.bind(this.url))();
	
};

URL.prototype.getErrorMessage = function() {
	
	var errorMessage = '';
	
	if (this.params['error'] !== undefined) {
		errorMessage += this.params['error'];
		errorMessage += ": ";
	}
	
	if (this.params['error_description'] !== undefined) {
		errorMessage += this.params['error_description'];
	}
	
	if (this.params['error_reason'] !== undefined) {
		errorMessage += (" (" + this.params['error_reason'] + ")");
	}
	
	return errorMessage;
};

URL.prototype.getAccesToken = function(app) {
	if (app.params.state !== undefined) {
		if (this.params.state !== app.params.state)
			return;
	}
	return this.params.access_token;
};

URL.prototype.search = function(pattern) {
	return this.url.search(pattern);
};