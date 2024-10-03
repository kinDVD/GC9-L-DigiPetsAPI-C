package co.grandcircus.account_api;


import java.util.Optional;
import java.util.UUID;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;



@RestController
public class AccountController {
    @Autowired
    private AccountsRepository accRepo;

    @GetMapping("/AccountDetails")
    public AccountDetails getAccount(@RequestParam String username, @RequestParam String apiKey) {
        return accRepo.getByUsernameAndApiKey(username, apiKey);
    }

    @GetMapping("/AccountDetails/{id}")
    public Optional<AccountDetails> getById(@PathVariable("id") Long id) {
        return accRepo.findById(id);
    }
    
    @GetMapping("/AccountDetails/by-key/{apiKey}")
    public AccountDetails getByApiKey(@PathVariable("apiKey") String apiKey) {
        return accRepo.findByApiKey(apiKey);
    }
    
    @PostMapping("/AccountDetails")
    public AccountDetails addAccount(@RequestBody AccountDetails entity) {
        UUID apiKeyUUID = UUID.randomUUID();

        String apiKey = apiKeyUUID.toString();

        entity.setApiKey(apiKey);
        entity.setId(null);

        accRepo.save(entity);
        return entity;
    }

    @DeleteMapping("/AccountDetails/{id}")
    public void deleteAccount(@PathVariable("id") Long id){
        accRepo.deleteById(id);
    }
    
    @PutMapping("/AccountDetails/{id}/add-credits")
    public int addCredits(@PathVariable("id") Long id, @RequestParam int amount){
        AccountDetails account = accRepo.findById(id).orElse(null);
        int credits = account.getCredits();
        credits += amount;
        return credits;
        //return accRepo.addCredits(id, amount);       
    }

    @PutMapping("/AccountDetails/{id}/action")
    public AccountDetails actionById(@PathVariable("id") Long id, @RequestParam String action){
        AccountDetails account = accRepo.findById(id).orElse(null);
        int credits = account.getCredits();
        int cost = 0;
        if(action == "CREATE"){
            cost = 5;
            credits -= cost;
        }
        else if(action == "HEAL"){
            cost = 1;
            credits -= cost;
        }
        else if(action == "TRAIN"){
            cost = 1;
            credits -= cost;
        }
        else if(action == "BATTLE"){
            cost = 2;
            credits -= cost;
        }
        account.setCredits(credits);
        return account;
    }
    
    @PostMapping("/AccountDetails/by-key/{apiKey}/action")
    public AccountDetails actionByApi(@PathVariable("apiKey") String apiKey, @RequestParam String action){
        AccountDetails account = accRepo.findByApiKey(apiKey);
        int credits = account.getCredits();
        int cost = 0;
        if(action == "CREATE"){
            cost = 5;
            credits -= cost;
        }
        else if(action == "HEAL"){
            cost = 1;
            credits -= cost;
        }
        else if(action == "TRAIN"){
            cost = 1;
            credits -= cost;
        }
        else if(action == "BATTLE"){
            cost = 2;
            credits -= cost;
        }
        account.setCredits(credits);
        return account;
    }
}
