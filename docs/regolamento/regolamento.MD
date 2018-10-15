# Regolamento

## Round di combattimento

L’unità nel round compie due azioni: azione di movimento e azione marziale. L’azione di movimento viene effettuata sempre per prima (l’azione marziale termina il round)

## Azioni di movimento

* Movimento: spostamento di 3 caselle
* Movimento tattico: spostamento di 1 casella, evitando gli attacchi di opportunità
* Utilizzo oggetti: apertura porte, leve, raccolta oggetti, ricaricare armi da tiro

## Azioni marziali:
* Attacco
* Guardia
* Cura
* Utilizzo di abilità attive specifiche

E’ possibile utilizzare un’azione di movimento al posto di un’azione marziale. 
E’ possibile rinunciare ad utilizzare una o entrambe le azioni.

## Attacchi d’opportunità

Qualsiasi azione diversa dal movimento tattico e dall’attacco corpo a corpo, eseguita in una casella “minacciata” da una o più unità avversarie, espone chi la compie ad un attacco di opportunità. 

## Abilità specifiche

### Passive

* Tiro
Permette di colpire avversari a distanza di 3 o 4 caselle. E’ necessaria una azione di movimento per ricaricare l’arma. E’ necessaria una linea di tiro pulita (nessuna unità alleata o nemica sulla linea di tiro)
* Arma a portata
Permette di colpire avversari a distanza di 2 caselle. Non è possibile utilizzare questa abilità se sulla linea di tiro c’è un avversario (è possibile invece farlo se c’è un alleato)

### Attive

* Tiro mirato (sostituisce “inchioda al terreno” che è inutile)
Impiegando un’azione di attacco, l’unità prende di mira un avversario, ad una distanza tra le 2 e le 5 caselle. Nella successiva azione di attacco, a patto che l’avversario sia ancora in range 2-5 caselle, si effettua un attacco ignorando tutte le abilità di difesa dell’avversario.
* Incalzare
Dopo l’uccisione di un avversario, è possibile effettuare un’azione di attacco aggiuntiva contro un’altra unità a portata.
* Difesa
Impiegando un’azione di attacco, l’unità “protegge” un compagno adiacente, negando il successivo attacco a lui indirizzato. Se l’unità “protetta” si sposta, perde questo vantaggio.

## Intelligenza artificiale

Le azioni dell’intelligenza artificiale sono volte a minimizzare la minaccia rappresentata dalle unità del giocatore. La minaccia di ogni unità del giocatore, calcolata per ogni unità del personaggio, è rappresentata da un numero che viene modificato in base alle azioni compiute dall’unità nemica:
*	Attacco di un compagno: 3 punto
*	Uccisione di un compagno: 5 punti
*	Attacco verso l’unità: 5 punti
*	Guardia: 2 punto 
*	Utilizzo di un’abilità neutra: 1 punto

Lo scopo dell’IA è sempre minimizzare la minaccia, quindi la logica da applicare è:
1.	Se in questo turno posso attaccare uno o più avversari (a prescindere dalla probabilità di riuscita di questa azione) senza incorrere in attacchi di opportunità, scelgo l’avversario con minaccia più alta e agisco per attaccarlo
2.	Se in questo turno non posso attaccare alcun avversario, ma posso avvicinarmi ad uno o più avversari senza incorrere in attacchi di opportunità, mi sposto fino a ridurre al minimo la distanza e poi attivo la guardia
3.	Se non posso nemmeno avvicinarmi, faccio un movimento tattico in direzione dell’avversario con minaccia più alta e poi attivo la guardia
4.	Se non posso fare un movimento tattico in direzione dell’avversario con minaccia più alta, torno al punto 2 selezionando il secondo bersaglio con minaccia più alta
5.	Se non posso avvicinarmi ad alcun avversario, non mi sposto e uso la guardia


