package co.grandcircus.account_api;

import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;

public class AccountDetails {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String username;
    private String apiKey;
    private int credits;
    
    public AccountDetails(Long id, String username, String apiKey, int credits) {
        this.id = id;
        this.username = username;
        this.apiKey = apiKey;
        this.credits = credits;
    }
    
    public AccountDetails() {
    }

    public Long getId() {
        return id;
    }
    public void setId(Long id) {
        this.id = id;
    }
    public String getUsername() {
        return username;
    }
    public void setUsername(String username) {
        this.username = username;
    }
    public String getApiKey() {
        return apiKey;
    }
    public void setApiKey(String apiKey) {
        this.apiKey = apiKey;
    }
    public int getCredits() {
        return credits;
    }
    public void setCredits(int credits) {
        this.credits = credits;
    }
    
}
