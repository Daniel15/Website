<?php defined('SYSPATH') or die(" {o,o}<br /> |)__)<br /> -\"-\"-<br />O RLY?!");
/**
 * Akismet class. Based on http://akismet.com/development/api/
 *
 * @package    Akismet
 * @author     Greenek.com <pgolonko@gmail.com>
 * @copyright  (c) 2009 Paweł Golonko
 * @license    BSD software license
 */
class Kohana_Akismet {

    // Akismet API version, server host and port
    const AKISMET_VERSION = '1.11';
    const AKISMET_HOST = 'rest.akismet.com';
    const AKISMET_PORT = 80;

    // Configuration settings
    protected $_config;

    // Connection
    protected $_connection;

    // User agent string
    protected static $_user_agent;

    // Comment
    protected $_comment = array();

    // API key
    protected $_key;

    /**
     * Creates a new Akismet object.
     *
     * @param   array  configuration
     * @return  Akismet
     */
    public static function factory(array $comment, array $config = array())
    {
        return new Akismet($comment, $config);
    }

    /**
     * Creates a new Akismet object.
     *
     * @param   array  configuration
     * @return  void
     */
    public function __construct(array $comment, array $config = array())
    {
        // Load configuration from Akismet config file
        $this->_config = Kohana::config('akismet');

        // Set base URL of page, required to verification.
        $this->_config['blog'] = URL::base(TRUE);

        // Add/overwrite with custom config values
        $this->setup($config);

        // Load and fill comments values.
        $this->_comment = $comment;
        $this->fill_comments_value();

        // Set user agent string
        Akismet::$_user_agent = 'Kohana/'.Kohana::VERSION.' | Akismet/'.Akismet::AKISMET_VERSION;

        // Verify key
        if ($this->verify_key())
        {
            $this->_key = $this->_config['key'];
        }
        else
        {
            throw new Exception("Your Akismet API key is not valid.");
        }
    }

    /**
     * Destroy this object.
     */
    public function __destruct()
    {
        if ($this->_connection)
        {
            $this->disconnect();
        }
    }

    /**
     * Update config values.
     *
     * @param   array   configuration
     * @return  object  Akismet
     */
    protected function setup(array $config = array())
    {
        if (isset($config['key']))
        {
            $this->_config['key'] = $config['key'];
        }

        if (isset($config['blog']))
        {
            $this->_config['blog'] = $config['blog'];
        }

        // Chainable method
        return $this;
    }

    /**
     * Establish connection to Akismet server.
     */
    protected function connect()
    {
        if ( ! ($this->_connection = @fsockopen(Akismet::AKISMET_HOST, Akismet::AKISMET_PORT)))
        {
            throw new Exception("Could not connect to akismet server.");
        }
    }

    /**
     * Close connection.
     */
    protected function disconnect()
    {
        @fclose($this->_connection);
    }

    /**
     * All calls to Akismet are POST requests much like a web form would send.
     * The request variables should be constructed like a query string,
     * key=value and multiple variables separated by ampersands. Don't forget to
     * URL escape the values.
     */
    protected function response($request, $path)
    {
        // Establish connection
        $this->connect();

        if ($this->_connection)
        {
            $http_request = "POST /1.1/$path HTTP/1.0\r\n"
                          . "Host: ".(( ! empty($this->_key)) ? $this->_key."." : NULL).Akismet::AKISMET_HOST."\r\n"
                          . "Content-Type: application/x-www-form-urlencoded; charset=utf-8\r\n"
                          . "Content-Length: ".strlen($request)."\r\n"
                          . "User-Agent: ".Akismet::$_user_agent."\r\n"
                          . "\r\n"
                          . $request;

            $response = '';

            @fwrite($this->_connection, $http_request);

            while( ! feof($this->_connection))
            {
                $response .= @fgets($this->_connection, 1160);
            }
            $response = explode("\r\n\r\n", $response, 2);

            return $response[1];
        }
        else
        {
            throw new Exception("The response could not be retrieved.");
        }

        $this->disconnect();
    }

    /**
     * The key verification call should be made before beginning to use the
     * service. It requires two variables, key and blog.
     *
     * key (required)
     *  The API key being verified for use with the API
     * blog (required)
     *  The front page or home URL of the instance making the request. For
     *  a blog, site, or wiki this would be the front page. Note: Must be a full
     *  URI, including http://.
     *
     * The call returns "valid" if the key is valid. This is the one call that
     * can be made without the API key subdomain.
     */
    public function verify_key()
    {
        $request = 'key='.$this->_config['key'].'&blog='.urlencode($this->_config['blog']);
        $response = $this->response($request, 'verify-key');		
        return ($response == 'valid');
    }

    /**
     * This is basically the core of everything. This call takes a number of
     * arguments and characteristics about the submitted content and then
     * returns a thumbs up or thumbs down. Almost everything is optional, but
     * performance can drop dramatically if you exclude certain elements. I
     * would recommend erring on the side of too much data, as everything is
     * used as part of the Akismet signature.
     *
     * blog (required)
     *  The front page or home URL of the instance making the request. For a
     *  blog or wiki this would be the front page. Note: Must be a full URI,
     *  including http://.
     * user_ip (required)
     *  IP address of the comment submitter.
     * user_agent (required)
     *  User agent information.
     * referrer (note spelling)
     *  The content of the HTTP_REFERER header should be sent here.
     * permalink
     *  The permanent location of the entry the comment was submitted to.
     * comment_type
     *  May be blank, comment, trackback, pingback, or a made up value like "registration".
     * comment_author
     *  Submitted name with the comment
     * comment_author_email
     *  Submitted email address
     * comment_author_url
     *  Commenter URL.
     * comment_content
     *  The content that was submitted.
     * Other server enviroment variables
     *  In PHP there is an array of enviroment variables called $_SERVER which
     *  contains information about the web server itself as well as a key/value
     *  for every HTTP header sent with the request. This data is highly useful
     *  to Akismet as how the submited content interacts with the server can be
     *  very telling, so please include as much information as possible.
     *
     * This call returns either "true" or "false" as the body content. True
     * means that the comment is spam and false means that it isn't spam. If you
     * are having trouble triggering you can send "viagra-test-123" as the
     * author and it will trigger a true response, always.
     *
     * @return  boolean
     */
    public function comment_check()
    {
        $request = $this->format_query_string();
        $response = $this->response($request, 'comment-check');
        return ($response == 'true');
    }

    /**
     * Alias for comment_check.
     */
    public function is_spam()
    {
        return $this->comment_check();
    }

    /**
     * This call is for submitting comments that weren't marked as spam but
     * should have been. It takes identical arguments as comment check.
     *
     * @return  void
     */
    public function submit_spam()
    {
        $request = $this->format_query_string();
        $response = $this->response($request, 'submit-spam');
    }

    /**
     * This call is intended for the marking of false positives, things that
     * were incorrectly marked as spam. It takes identical arguments as comment
     * check and submit spam.
     *
     * @return  void
     */
    public function submit_ham()
    {
        $request = $this->format_query_string();
        $response = $this->response($request, 'submit-ham');
    }

    /**
     * Fill required values if not exists.
     *
     * @return  void
     */
    protected function fill_comments_value()
    {
        if ( ! isset($this->_comment['blog']))
        {
            $this->_comment['blog'] = $this->_config['blog'];
        }

        if ( ! isset($this->_comment['user_ip']))
        {
            $this->_comment['user_ip'] = ($_SERVER['REMOTE_ADDR'] != getenv('SERVER_ADDR')) ? $_SERVER['REMOTE_ADDR'] : getenv('HTTP_X_FORWARDED_FOR');
        }

        if ( ! isset($this->_comment['user_agent']))
        {
            $this->_comment['user_agent'] = $_SERVER['HTTP_USER_AGENT'];
        }

        if ( ! isset($this->_comment['referrer']) AND ! empty($_SERVER['HTTP_REFERER']))
        {
            $this->_comment['referrer'] = $_SERVER['HTTP_REFERER'];
        }
    }

    /**
     * Prepare comment array to sending.
     */
    protected function format_query_string()
    {
        $query = '';

        foreach ($this->_comment as $key => $value)
        {
            $query .= $key.'='.urlencode(stripslashes($value)).'&';
        }

        return $query;
    }

}
