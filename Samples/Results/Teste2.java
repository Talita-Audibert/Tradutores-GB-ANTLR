public class Teste2{ 
	 public static void main(String[] args){ 
 		Scanner entrada = new Scanner(System.in);
		double n1, n2, saida;
		char oper;
		System.out.println();
		System.out.println("Insira o primeiro num");
		n1 = entrada.next();
		System.out.println("Operacao");
		oper = entrada.next();
		System.out.println("Insira o segundo num");
		n2 = entrada.next();
		System.out.println();
		switch(oper){
		case"+":
			saida=n1+n2;
		break;
		case"-":
			saida=n1-n2;
		break;
		case"*":
		if(n2=0){
			System.out.println("Erro! Divisao por zero, entre com um denominador diferente de 0");
			}
		else{
			saida=n1/n2;
		}
		break;
		case"/":
			saida=n1*n2;
		break;
		}
		if(a=b){
			teste;
		}
		System.out.println("Resultado da operação:");
		System.out.println();
		entrada.close(); 
 	 } 
 }