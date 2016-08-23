/**
 * dan.cx JavaScript (revision 2) - By Daniel15, 2011-2014
 * "Social feed" / lifestream
 * Feel free to use any of this, but please link back to my site
 */

(function(global) {
	'use strict';

	var SocialFeed = React.createClass({
		getDefaultProps: function() {
			return {
				count: 10,
				feedUrl: 'socialfeed/loadjson.php',
				showDescription: false,
				className: 'socialfeed',
				showLoadMore: true
			};
		},
		getInitialState: function() {
			return {
				data: this.props.initialData || []
			};
		},
		componentDidMount: function() {
			// If no initial data was passed, do an AJAX load
			if (!this.state.data || this.state.data.length === 0) {
				this._loadNextPage();
			}
		},
		render: function() {
			return (
				<div>
					<ul className={this.props.className}>
						{this.state.data.map(item =>
							<FeedItem
								id={item.id}
								key={item.id}
								text={item.text}
								subtext={item.subtext}
								description={this.props.showDescription ? item.description : null}
								type={item.type}
								url={item.url}
								date={new Date(item.date * 1000)}
								relativeDate={item.relativeDate}
							/>
						)}
					</ul>
					{this._renderShowMore()}
				</div>
			);
		},
		_renderShowMore: function() {
			if (!this.props.showLoadMore) {
				return null;
			}
			if (this.state.loading || this.state.data.length === 0) {
				return (
					<span>Loading...</span>
				);
			}
			var lastItem = this.state.data[this.state.data.length - 1];
			var url = 'socialfeed.htm?before_date=' + lastItem.date;
			return (
				<a href={url} onClick={this._loadNextPage}>
					Show more!
				</a>
			);
		},
		/**
		 * Load the data for the lifestream
		 */
		_loadNextPage: function(evt) {
			this.setState({ loading: true });
			var data =
			{
				count: this.props.count
			};

			if (this.state.data && this.state.data.length !== 0) {
				var lastItem = this.state.data[this.state.data.length - 1];
				data.before_date = lastItem.date;
			}

			new Ajax(this.props.feedUrl, {
				onSuccess: this._onLoadSuccess,
				context: this,
				data: data
			}).send();

			if (evt) {
				evt.preventDefault();
			}
		},
		/**
		 * Called when the AJAX request is successful
		 * @param	Data returned from server
		 */
		_onLoadSuccess: function(response) {
			this.setState({
				loading: false,
				data: this.state.data.concat(response)
			});
		}
	});

	var FeedItem = React.createClass({
		render: function() {
			return (
				<li className={'feeditem source-' + this.props.type}>
					<div className="icon" />
					<span dangerouslySetInnerHTML={{__html: this.props.text}} />
					{this._renderDescription()}

					<ul className="meta" title={'Via ' + this.props.type}>
						{this._renderMeta()}
					</ul>
				</li>
			);
		},

		_renderDescription: function() {
			return this.props.description
				? <blockquote
					dangerouslySetInnerHTML={{__html: this.props.description}}
				/> : null
		},

		_renderMeta: function() {
			var meta = [
				<li key="date" className="date">
					{this.props.relativeDate}
				</li>
			];
			if (this.props.subtext) {
				meta.push(
					<li
						key="subtext"
						className="subtext"
						dangerouslySetInnerHTML={{__html: this.props.subtext}}
					/>
				);
			}
			if (this.props.url) {
				meta.push(
					<li key="url">
						<a href={this.props.url} target="_blank">View</a>
					</li>
				);
			}
			return meta;
		}
	});

	global.SocialFeed = SocialFeed;
}(this));
