var firstline;

function OnMouseDown () {
	
	//����� � �������
	var scr = GameObject.Find("BoardGroup").GetComponent(BoardScript);
	var s : String[] = new String[scr.qText.Length];
	var i : int;
	
	for (i=0; i<scr.qText.Length; i++) {
		s[i] = "Q"+(i+1)+": "+scr.a[i,0]+" "+scr.a[i,1]+" "+scr.a[i,2];
		if (scr.qAnsNum[i] >= 4) s[i] = s[i]+" "+scr.a[i,3];
		if (scr.qAnsNum[i] == 5) s[i] = s[i]+" "+scr.a[i,4];
	}
	
	for (i=0; i<scr.qText.length; i++) {
		Application.ExternalEval("document.write('<p>"+s[i]+"</p>');");
	}
	
	//���� �� ���������� �����
	//��������� ����� ������ ���� � "UTF-8 ��� BOM", � �� ������ "UTF-8"
	//��� �����, ��, ����� ����� �� ����� ���������
	/*�� ������ ������
	var scr = GameObject.Find("BoardGroup").GetComponent("BoardScript");
	var test : WWW = new WWW ("file://E:/Unity3D/Test.txt");
	yield test;
	var words = test.text.Split("\n"[0]);
	var k : int = System.Int32.Parse(words[0]);*/
}