package co.grandcircus.account_api;

import org.springframework.data.jpa.repository.JpaRepository;

public interface AccountsRepository extends JpaRepository<AccountDetails, Long> {
    AccountDetails getByUsernameAndApiKey(String username, String apiKey);

    AccountDetails findByApiKey(String apiKey);

    //AccountDetails addCredits(Long id, int amount);


}
