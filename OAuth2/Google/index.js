var app = 
{
	params : 
	{
		client_id : '760921016908-32jtmqtojn7aja9qvt4t7mh0089er9tb.apps.googleusercontent.com', 
		redirect_uri : 'https://localhost:443/OAuth2/index.html',
		response_type : 'token',
		scope : 'https://www.googleapis.com/auth/plus.login https://www.googleapis.com/auth/userinfo.email',
		state : 'abcd'
	},
	
	provider : 
	{
		name: 'Google',
		authServerUrl: 'https://accounts.google.com/o/oauth2/auth?',
		resourceServerUrl: 'https://www.googleapis.com/plus/v1/people/me?'
	},
	
	fields : 'emails,displayName,name(givenName)',
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
				
				if (prop[0] !== 'access_token')
				{
					prop[1] = prop[1]
					.replace(/\+/g, ' ')
					.replace(/\_/g, ' ');
				}
				
				p[prop[0]] = prop[1];
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
		
		$('#welcomeLabel').attr('title', user.displayName);
		$('#emailLink').text(user.name.givenName);
		
		if (user.emails !== undefined && user.emails.length > 0) 
			$('#emailLink').attr('href', 'mailto:' + user.emails[0].value);
		
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