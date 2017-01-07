var app = 
{
	params : 
	{
		client_id : '1654158091578578', 
		redirect_uri : 'https://localhost:443/OAuth2/index.html',
		response_type : 'token',
		scope : 'public_profile email',
		state : 'abcd'
	},
	
	provider : 
	{
		name: 'Facebook',
		authServerUrl: 'https://www.facebook.com/dialog/oauth?',
		resourceServerUrl: 'https://graph.facebook.com/me?'
	},
	
	fields : 'name,first_name,last_name,email',
	httpMethod: 'GET'
};

$(document).ready(function() {
	var page = new Page(app);
	page.display();
});

var Page = function(app) {
	
	this.url = { url : window.location.href };
	this.app = app;
	this.user = undefined;
	
	this.url.params = (function() {
		
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
		
	}.bind(this.url.url))();
	
	this.url.getErrorMessage = function() {
		
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
	
	this.url.getAccesToken = function(app) {
		if (app.params.state !== undefined) {
			if (this.params.state !== app.params.state)
				return;
		}
		return this.params.access_token;
	};
	
	this.url.search = function(pattern) {
		return this.url.search(pattern);
	}
	
	this.display = function() {
		
		$('#welcomeLabel').hide();
		
		if (this.url.search(/\?{1}.*error=/i) >= 0) {
			// If the url has a URI fragment named error
			var errorMessage = this.url.getErrorMessage();
			
			this.displayErrorMessage(errorMessage);
		}
		else if (this.url.search(/#.*access_token=/i) >= 0) {
			// If the url has a URI fragment named access_token
			try {
				var accessToken = this.url.getAccesToken(this.app);
				
				this.getUser(accessToken, this.app);
				
				if (this.user === undefined) {
					throw new Error("Unable to fetch user details from " + this.app.provider.name + ".");
				}
				this.displayWelcomeMessage(this.user);
			}
			catch(e) {
				this.displayErrorMessage(e.message);
			}
		}
		else {
			// otherwise, display the login link
			this.displayLoginUrl(this.app);
		}
	};
	
	this.displayLoginUrl = function(app) {
		var loginUrl = this.getLoginUrl(app);
		$('#loginLink').attr('href', loginUrl);
		
		$('#loginLink').show();
		$('#welcomeLabel').hide();
		$('#errorLabel').hide();
	};
	
	this.displayWelcomeMessage = function(user) {
		
		$('#welcomeLabel').attr('title', user.name);
		$('#emailLink').text(user.first_name);
		
		if (user.email !== undefined) 
			$('#emailLink').attr('href', 'mailto:' + user.email);
		
		$('#loginLink').hide();
		$('#welcomeLabel').show();
		$('#errorLabel').hide();
	};
	
	this.getLoginUrl = function(app) {
		var loginUrl = app.provider.authServerUrl;
		
		Object.keys(app.params).forEach(function(key) {
			loginUrl += (key + '=' + app.params[key] + '&');
		});
		
		return loginUrl;
	};
	
	this.displayErrorMessage = function(errorMessage) {
		$('#loginLink').hide();
		$('#welcomeLabel').hide();
		$('#errorLabel').text(errorMessage);
		$('#errorLabel').show();
	};
	
	this.getUser = function(accessToken, app) {
		var resourceServerUrl = app.provider.resourceServerUrl + "access_token=" + accessToken + "&fields=" + app.fields;
		
		var that = this;
		
		$.ajax({
			url : resourceServerUrl,
			type: app.httpMethod,
			async : false,
			success : function(data) {
				that.user = data;
			}
		});
	};
};