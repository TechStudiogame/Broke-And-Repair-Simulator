using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairGame : MonoBehaviour
{
    Canvas canvas; Font font; RectTransform root;
    Text title, subtitle, moneyText, bodyText;
    readonly List<GameObject> dynamicObjects = new List<GameObject>();
    int money = 250, phoneIndex = 0, phase = 0, selectedPart = 0;
    string[] phones = {"iPhone 18 Pro", "Galaxy S26 Ultra", "Pixel 11 Pro"};
    string[] parts = {"Экран", "Аккумулятор", "Камера", "Разъём зарядки"};
    string[] phases = {"Диагностика", "Разборка", "Замена детали", "Сборка", "Проверка"};
    bool repaired = false;

    void Start() { BuildUI(); ShowHome(); }
    void BuildUI()
    {
        font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        var go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvas = go.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = go.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920); scaler.matchWidthOrHeight = 0.5f;
        root = MakeRect("Root", go.transform, new Vector2(0,0), new Vector2(1,1), Vector2.zero, Vector2.zero);
        Image bg = root.gameObject.AddComponent<Image>(); bg.color = new Color(.025f,.11f,.32f);
        title = MakeText("TITLE", root, 48, new Color(1f,.76f,.16f), new Vector2(.05f,.88f), new Vector2(.95f,.98f), TextAnchor.MiddleCenter);
        subtitle = MakeText("", root, 28, Color.white, new Vector2(.06f,.81f), new Vector2(.94f,.88f), TextAnchor.MiddleCenter);
        moneyText = MakeText("", root, 26, new Color(.4f,1f,.62f), new Vector2(.05f,.75f), new Vector2(.95f,.81f), TextAnchor.MiddleCenter);
        bodyText = MakeText("", root, 30, Color.white, new Vector2(.08f,.45f), new Vector2(.92f,.74f), TextAnchor.MiddleCenter);
    }
    RectTransform MakeRect(string name, Transform parent, Vector2 amin, Vector2 amax, Vector2 offMin, Vector2 offMax)
    {
        var g = new GameObject(name, typeof(RectTransform)); g.transform.SetParent(parent,false);
        var r = g.GetComponent<RectTransform>(); r.anchorMin=amin; r.anchorMax=amax; r.offsetMin=offMin; r.offsetMax=offMax; return r;
    }
    Text MakeText(string value, Transform parent, int size, Color color, Vector2 amin, Vector2 amax, TextAnchor align)
    {
        var r=MakeRect("Text",parent,amin,amax,Vector2.zero,Vector2.zero);
        var t=r.gameObject.AddComponent<Text>(); t.font=font; t.fontSize=size; t.color=color; t.alignment=align;
        t.horizontalOverflow=HorizontalWrapMode.Wrap; t.verticalOverflow=VerticalWrapMode.Overflow; t.text=value; return t;
    }
    void ClearButtons(){ foreach(var o in dynamicObjects) Destroy(o); dynamicObjects.Clear(); }
    void Button(string label, float y, Action action, Color? color=null)
    {
        var r=MakeRect("Button_"+label,root,new Vector2(.12f,y),new Vector2(.88f,y+.075f),Vector2.zero,Vector2.zero);
        var im=r.gameObject.AddComponent<Image>(); im.color=color ?? new Color(.05f,.25f,.58f);
        var b=r.gameObject.AddComponent<Button>(); b.onClick.AddListener(()=>action());
        var t=MakeText(label,r,28,Color.white,new Vector2(.03f,.05f),new Vector2(.97f,.95f),TextAnchor.MiddleCenter);
        dynamicObjects.Add(r.gameObject);
    }
    void Header(string sub){ title.text="PHONE REPAIR"; subtitle.text=sub; moneyText.text="Баланс: $"+money; }
    void ShowHome()
    {
        ClearButtons(); Header("SIMULATOR");
        bodyText.text="Ремонтируй телефоны, меняй детали\nи проверяй результат.\n\nВыбирай заказ и начинай работу.";
        Button("НАЧАТЬ РЕМОНТ",.32f,()=>{phoneIndex=0;phase=0;repaired=false;ShowPhone();},new Color(.05f,.42f,.25f));
        Button("МАГАЗИН ДЕТАЛЕЙ",.22f,ShowShop);
        Button("СБРОСИТЬ ПРОГРЕСС",.12f,()=>{money=250;ShowHome();});
    }
    void ShowPhone()
    {
        ClearButtons(); Header("ЗАКАЗ: "+phones[phoneIndex]);
        bodyText.text="Телефон: "+phones[phoneIndex]+"\n\nСостояние: "+(repaired?"Исправен":"Неисправен")+
            "\nЭтап: "+phases[Mathf.Clamp(phase,0,4)]+"\n\n"+(phase==0?"Проведи диагностику.":phase==1?"Разбери корпус и отключи компоненты.":phase==2?"Выбери неисправную деталь и замени её.":phase==3?"Собери телефон обратно.":"Проверь устройство."));
        if(phase==0) Button("ПРОВЕСТИ ДИАГНОСТИКУ",.32f,()=>{phase=1;ShowPhone();});
        else if(phase==1) Button("РАЗОБРАТЬ ТЕЛЕФОН",.32f,()=>{phase=2;ShowPhone();});
        else if(phase==2)
        {
            for(int i=0;i<parts.Length;i++){int ix=i;Button("Заменить: "+parts[i],.36f-i*.085f,()=>{selectedPart=ix;phase=3;ShowPhone();});}
        }
        else if(phase==3) Button("СОБРАТЬ ТЕЛЕФОН",.32f,()=>{phase=4;ShowPhone();});
        else Button("ЗАПУСТИТЬ ТЕСТ",.32f,()=>{repaired=true;money+=180;bodyText.text="ТЕСТ ПРОЙДЕН!\n"+phones[phoneIndex]+" работает.\n\nЗаработано: $180";phase=0;});
        Button("СЛЕДУЮЩИЙ ЗАКАЗ",.20f,()=>{phoneIndex=(phoneIndex+1)%phones.Length;phase=0;repaired=false;ShowPhone();});
        Button("В МЕНЮ",.10f,ShowHome);
    }
    void ShowShop()
    {
        ClearButtons(); Header("МАГАЗИН");
        bodyText.text="Запчасти и инструменты\n\nПокупай детали для ремонта.\nБаланс: $"+money;
        Button("Экран — $60",.34f,()=>Buy(60,"Экран"));
        Button("Аккумулятор — $40",.25f,()=>Buy(40,"Аккумулятор"));
        Button("Камера — $50",.16f,()=>Buy(50,"Камера"));
        Button("НАЗАД",.08f,ShowHome);
    }
    void Buy(int cost,string name){ if(money>=cost){money-=cost;bodyText.text="Куплено: "+name+"\n";}else bodyText.text="Недостаточно денег."; moneyText.text="Баланс: $"+money; }
}