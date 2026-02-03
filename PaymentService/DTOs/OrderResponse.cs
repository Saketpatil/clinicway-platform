namespace PaymentService.DTOs;

public record OrderResponse(
    string orderId,
    long amount,
    string currency
);
