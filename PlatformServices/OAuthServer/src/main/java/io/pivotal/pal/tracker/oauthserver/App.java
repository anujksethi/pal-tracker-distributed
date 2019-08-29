package io.pivotal.pal.tracker.oauthserver;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.security.oauth2.config.annotation.configurers.ClientDetailsServiceConfigurer;
import org.springframework.security.oauth2.config.annotation.web.configuration.AuthorizationServerConfigurerAdapter;
import org.springframework.security.oauth2.config.annotation.web.configuration.EnableAuthorizationServer;
import org.springframework.security.oauth2.config.annotation.web.configurers.AuthorizationServerSecurityConfigurer;

@EnableAuthorizationServer
@SpringBootApplication
public class App extends AuthorizationServerConfigurerAdapter {
    public static void main(String[] args) {
        SpringApplication.run(App.class, args);
    }

    @Override
    public void configure(ClientDetailsServiceConfigurer clients) throws Exception {
        clients.inMemory()
            .withClient("tracker-client")
            .secret("supersecret")
            .authorizedGrantTypes("client_credentials")
            .scopes("openid");
    }

    @Override
    public void configure(AuthorizationServerSecurityConfigurer oauthServer)
        throws Exception {
        oauthServer
            .tokenKeyAccess("permitAll()")
            .checkTokenAccess("isAuthenticated()");
    }
}
