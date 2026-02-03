namespace PaymentService.DTOs;

public record PaymentVerificationRequest(
    string razorpayOrderId,
    string razorpayPaymentId,
    string razorpaySignature
);
