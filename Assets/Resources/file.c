//
//  file.c
//
//
//  Created by x24037xx on 2025/06/16.
//

#include <stdio.h>
#include <stdlib.h>

#define FILE_NAME "./dataLiDar.txt"

int main(void)
{
    FILE *fp;
    int zahyou;
    double x=10000.0,y=10000.0;
    
    if((fp = fopen(FILE_NAME, "w")) == NULL){
        printf("ファイルがオープンできません\n");
        exit(1);
    }
    
    //無限ループ(1〜9の間は入力を行える)
    while(1){
        //座標を入力
        scanf("%d", &zahyou);
        
        if(zahyou<1 || 9<zahyou){
            break;
        }else{
            //ファイルの初期化(空にする)
            fp = fopen(FILE_NAME, "w");
        }
        
        //書き込み完了ログ
        printf("書き込み完了\n");
         
        //switchで座標の値を決定
        switch (zahyou) {
            case 1:
                x=1.0;
                y=1.0;
                break;
                
            case 2:
                x=2.0;
                y=2.0;
                break;
                
            case 3:
                x=3.0;
                y=3.0;
                break;
                
            case 4:
                x=4.0;
                y=4.0;
                break;
                
            case 5:
                x=5.0;
                y=5.0;
                break;
                
            case 6:
                x=6.0;
                y=6.0;
                break;
                
            case 7:
                x=7.0;
                y=7.0;
                break;
                
            case 8:
                x=8.0;
                y=8.0;
                break;
                
            case 9:
                x=9.0;
                y=9.0;
                break;
                
            default:
                break;
        }
        
        //ファイルに書き込み
        fprintf(fp, "%f %f\n", x, y);
        fclose(fp);
        
        //中身の確認
        fp = fopen(FILE_NAME, "r");
        fscanf(fp, "%lf %lf", &x,&y);
        printf("x = %f, y = %f\n", x, y);
        fclose(fp);
    }

    return 0;
}
