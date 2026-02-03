namespace PaymentService.DTOs;

public record OrderRequest(
    long amount,
    string currency
);
