# Regolamento

## Round di combattimento

L’unità nel round compie due azioni: azione di movimento e azione marziale. L’azione di movimento viene effettuata sempre per prima (l’azione marziale termina il round)

## Azioni di movimento

* Movimento: spostamento di 3 caselle
* Movimento tattico: spostamento di 1 casella, evitando gli attacchi di opportunità
* Colpo di grazia: effettua un attacco su un avversario incapacitato, uccidendolo
* Utilizzo oggetti: apertura porte, leve, raccolta oggetti, ricaricare armi da tiro

## Azioni marziali:
* Attacco: effettua un attacco verso un nemico a portata
* Guardia: prepara un attacco di opportunità
* Cura: ristabilisce un compagno ferito. Consuma sia un'azione di movimento che un'azione marziale. L'unità ristabilita non può agire nello stesso round.
* Utilizzo di abilità attive specifiche

E’ possibile utilizzare un’azione di movimento al posto di un’azione marziale. 
E’ possibile rinunciare ad utilizzare una o entrambe le azioni.

## Attacchi d’opportunità

Qualsiasi azione diversa dal movimento tattico e dall’attacco corpo a corpo, eseguita in una casella “minacciata” da una o più unità avversarie, espone chi la compie ad un attacco di opportunità. 
L'effetto dell'attacco di opportunità si risolve sempre DOPO l'effetto dell'azione stessa (opzione per talento: "rapido" - i tuoi attacchi di opportunità si risolvono PRIMA delle azioni che li scatenano).

## Abilità specifiche

### Passive

* Tiro
Permette di colpire avversari a distanza di 3 o 4 caselle. E’ necessaria una azione di movimento per ricaricare l’arma. E’ necessaria una linea di tiro pulita (nessuna unità alleata o nemica sulla linea di tiro). 
Modifica anche la gestione dell'attacco di opportunità. Nel momento in cui si usa l'azione "guardia" si sceglie una linea di tiro (N/S/W/E) da presidiare, e solo su quella linea è possibile effettuare un attacco

Esempi (N e W)

00000000000		00000000000
00000x00000		00000000000
0000xxx0000		00000000000
000xx0xx000		000x0000000
00000000000		00xx0000000
00000U00000		0xx00U00000
00000000000		00xx0000000
00000000000		000x0000000
00000000000		00000000000
00000000000		00000000000
00000000000		00000000000

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
*	Attacco ad un compagno: 3 punti
*	Riduzione di un compagno ad incapacitato: 5 punti
*   Colpo di grazia ad un compagno: 10 punti
*	Attacco verso l’unità: 5 punti
*   Riduzione ad incapacitato: 10 punti
*	Guardia: 2 punti 
*	Utilizzo di un’abilità neutra: 1 punto
*   Cura di un avversario incapacitato: 5 punti

Lo scopo dell’IA è sempre minimizzare la minaccia, quindi la logica da applicare è:
1.  Se in questo turno posso dare il colpo di grazia ad un avversario incapacitato, senza incorrere in attacchi di opportunità, lo faccio scegliendo con priorità il bersaglio con minaccia più alta.
2.	Se in questo turno posso attaccare uno o più avversari (a prescindere dalla probabilità di riuscita di questa azione) senza incorrere in attacchi di opportunità, scelgo l’avversario con minaccia più alta che posso attaccare e lo faccio.
3.	Se in questo turno non posso attaccare alcun avversario, ma posso avvicinarmi ad uno o più avversari senza incorrere in attacchi di opportunità, mi sposto fino a ridurre al minimo la distanza verso l'avversario più vicino e poi attivo la guardia
4.	Se non posso nemmeno avvicinarmi, faccio un movimento tattico in direzione dell’avversario più vicino e poi attivo la guardia
5.	Se non posso avvicinarmi ad alcun avversario, non mi sposto e uso la guardia


