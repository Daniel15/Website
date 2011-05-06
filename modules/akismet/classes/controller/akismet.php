<?php defined('SYSPATH') or die(" {o,o}<br /> |)__)<br /> -\"-\"-<br />O RLY?!");

class Controller_Akismet extends Controller {

    public function action_index()
    {
        $comment = array
        (
            'comment_author'    => 'Author',
            'comment_content'   => 'Content',
        );

        $akismet = Akismet::factory($comment)->is_spam();
        echo Debug::dump($akismet);
    }

}
