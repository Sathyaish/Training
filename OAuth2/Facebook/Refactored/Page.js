var Page = function(app) {
	
	this.app = app;
	this.user = undefined;
	
	this.url = new URL(window.location.href);
};

Page.prototype.display = function() {
		
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

Page.prototype.displayLoginUrl = function(app) {
	var loginUrl = this.getLoginUrl(app);
	$('#loginLink').attr('href', loginUrl);
	
	$('#loginLink').show();
	$('#welcomeLabel').hide();
	$('#errorLabel').hide();
};

Page.prototype.displayWelcomeMessage = function(user) {
	
	$('#welcomeLabel').attr('title', user.name);
	$('#emailLink').text(user.first_name);
	
	if (user.email !== undefined) 
		$('#emailLink').attr('href', 'mailto:' + user.email);
	
	$('#loginLink').hide();
	$('#welcomeLabel').show();
	$('#errorLabel').hide();
};

Page.prototype.getLoginUrl = function(app) {
	var loginUrl = app.provider.authServerUrl;
	
	Object.keys(app.params).forEach(function(key) {
		loginUrl += (key + '=' + app.params[key] + '&');
	});
	
	return loginUrl;
};

Page.prototype.displayErrorMessage = function(errorMessage) {
	$('#loginLink').hide();
	$('#welcomeLabel').hide();
	$('#errorLabel').text(errorMessage);
	$('#errorLabel').show();
};

Page.prototype.getUser = function(accessToken, app) {
	var resourceServerUrl = app.provider.resourceServerUrl + "access_token=" + accessToken + "&fields=" + app.fields;
	
	var thePage = this;
	
	$.ajax({
		url : resourceServerUrl,
		type: app.httpMethod,
		async : false,
		success : function(data) {
			thePage.user = data;
		}
	});
};